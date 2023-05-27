using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    public class MainPageOffersSearcher : ISearcher<JobOffer, MainPageOfferSearchParameters>
    {
        public MainPageOfferSearchParameters SearchParams { get; }
        public int AfterFiltersCount { get; set; }


        public MainPageOffersSearcher(MainPageOfferSearchParameters searchParams)
        {
            SearchParams = searchParams;
            AfterFiltersCount = 0;
        }

        public MainPageOffersSearcher()
        {
            SearchParams = new MainPageOfferSearchParameters();
            AfterFiltersCount = 0;
        }

        public async Task<List<JobOffer>> Search(IRepository<JobOffer> repository)
        {
            var offersSet = await repository.GetAllSet();
            var available = offersSet.Where(offer =>
                 !offer.IsDeleted &&
                 offer.IsPaid &&
                 !offer.IsSuspended &&
                 !offer.IsShelved &&
                 offer.IsPublished);

            var filtered = GetFiltered(available);
            AfterFiltersCount = filtered.Count();

            var sorted = GetSorted(filtered);

            int page = SearchParams.Page;
            int pageSize = SearchParams.PageSize;

            var pageOffers = sorted.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var loader = new LoadOffers(pageOffers);
            var loaded = await loader.Load();

            return loaded;
        }

        private IQueryable<JobOffer> GetFiltered(IQueryable<JobOffer> available)
        {
            if (SearchParams.IsRemoteOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.WorkLocation)
                    .Where(offer => offer.WorkLocation.Type == fullyRemoteString);
            }
            if (SearchParams.IsSalaryOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange)
                    .Where(offer => offer.JobOfferEmploymentDetails.Any(x => x.SalaryRange != null));
            }
            if (SearchParams.MainTechnology != MainPageOfferSearchParameters.AllTechnologiesIndex)
            {
                available = available.Where(offer => offer.MainTechnologyTypeId == SearchParams.MainTechnology);
            }
            if (!string.IsNullOrEmpty(SearchParams.SearchString))
            {
                available = SearchByKeywords(available);
            }

            return available;
        }

        private IQueryable<JobOffer> SearchByKeywords(IQueryable<JobOffer> available)
        {
            string search = SearchParams.SearchString!.Trim();
            string[] keywordsTokens = search.Split().Select(x => x.ToLower())
                .ToArray();

            available = available.Include(offer => offer.CompanyProfile)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords);

            // ranking + filtering
            // TODO: integrate Elasticsearch instead (SQL to store, Elasticsearch for queries)
            available = available
                .Select(x => new
                {
                    Offer = x,
                    Score = (keywordsTokens.Any(token => x.JobTitle == token) ? 8 : 0) +
                            // (keywordsTokens.Count(token => x.TechKeywords.Any(y => y.Name == token)) * 3) +
                            (keywordsTokens.Any(token => x.MainTechnologyType.Type == token) ? 4 : 0) +
                            (keywordsTokens.Any(token => x.Country == token) ? 3 : 0) +
                            (keywordsTokens.Any(token => x.City == token) ? 2 : 0) +
                            (keywordsTokens.Any(token => x.CompanyProfile.CompanyName == token) ? 1 : 0)
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Offer);

            return available;
        }

        private IQueryable<JobOffer> GetSorted(IQueryable<JobOffer> available)
        {
            available = available.OrderByDescending(offer => offer.PublishedAt);
            return available;
        }
    }
}
