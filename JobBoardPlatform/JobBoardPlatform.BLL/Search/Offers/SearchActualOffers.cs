using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Search.Offers
{
    public class SearchActualOffers : ISearcher<List<JobOffer>>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly OfferSearchDataUrlFactory searchDataFactory;
        private readonly int pageSize;

        public int AfterFiltersCount { get; set; }


        public SearchActualOffers(IRepository<JobOffer> repository, HttpRequest request, int pageSize) 
        {
            this.repository = repository;
            this.searchDataFactory = new OfferSearchDataUrlFactory(request);
            this.pageSize = pageSize;
        }

        public async Task<List<JobOffer>> Search()
        {
            var offersSet = await repository.GetAllSet();
            var available = offersSet.Where(offer =>
                 !offer.IsDeleted &&
                 offer.IsPaid &&
                 !offer.IsSuspended &&
                 !offer.IsShelved &&
                 offer.IsPublished);

            var searchData = searchDataFactory.Create();
            var filtered = GetFiltered(searchData, available);

            var sorted = GetSorted(searchData, filtered);

            int page = searchData.Page;
            var pageOffers = sorted.Skip((page - 1) * pageSize)
                .Take(pageSize);

            AfterFiltersCount = sorted.Count();

            var loader = new LoadActualOffersPage(pageOffers);
            var loaded = await loader.Load();

            return loaded;
        }

        private IQueryable<JobOffer> GetFiltered(OfferSearchData searchData, IQueryable<JobOffer> available) 
        {
            if (searchData.IsRemoteOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.WorkLocation)
                    .Where(offer => offer.WorkLocation.Type == fullyRemoteString);
            }
            if (searchData.IsSalaryOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange)
                    .Where(offer => offer.JobOfferEmploymentDetails.Any(x => x.SalaryRange != null));
            }
            if (searchData.MainTechnology != 0)
            {
                available = available.Where(offer => offer.MainTechnologyTypeId == searchData.MainTechnology);
            }
            if (!string.IsNullOrEmpty(searchData.SearchString))
            {
                available = SearchByKeywords(searchData, available);
            }

            return available;
        }

        private IQueryable<JobOffer> GetSorted(OfferSearchData searchData, IQueryable<JobOffer> available)
        {
            available = available.OrderByDescending(offer => offer.PublishedAt);
            return available;
        }

        private IQueryable<JobOffer> SearchByKeywords(OfferSearchData searchData, IQueryable<JobOffer> available)
        {
            string search = searchData.SearchString!.Trim();
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
    }
}
