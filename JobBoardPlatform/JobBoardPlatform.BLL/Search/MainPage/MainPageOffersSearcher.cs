using JobBoardPlatform.BLL.Search.Templates;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    public class MainPageOffersSearcher : FilteringPageSearcherBase<JobOffer, MainPageOfferSearchParams>
    {
        private readonly IRepository<JobOffer> repository;


        public MainPageOffersSearcher(IRepository<JobOffer> repository)
        {
            this.repository = repository;
        }

        protected override IRepository<JobOffer> GetRepository()
        {
            return repository;
        }

        protected override IQueryable<JobOffer> GetFiltered(IQueryable<JobOffer> available)
        {
            available = available.Where(offer =>
                 !offer.IsDeleted &&
                 offer.IsPaid &&
                 !offer.IsSuspended &&
                 !offer.IsShelved &&
                 offer.IsPublished);

            if (searchParams.IsRemoteOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.WorkLocation)
                    .Where(offer => offer.WorkLocation.Type == fullyRemoteString);
            }
            if (searchParams.IsSalaryOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange)
                    .Where(offer => offer.JobOfferEmploymentDetails.Any(x => x.SalaryRange != null));
            }
            if (searchParams.MainTechnology != MainPageOfferSearchParams.AllTechnologiesIndex)
            {
                available = available.Where(offer => offer.MainTechnologyTypeId == searchParams.MainTechnology);
            }
            if (!string.IsNullOrEmpty(searchParams.SearchString))
            {
                available = SearchByKeywords(available);
            }

            return available;
        }

        protected override IQueryable<JobOffer> GetSorted(IQueryable<JobOffer> available)
        {
            available = available.OrderByDescending(offer => offer.PublishedAt);
            return available;
        }

        private IQueryable<JobOffer> SearchByKeywords(IQueryable<JobOffer> available)
        {
            string search = searchParams.SearchString!.Trim();
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

        protected override IEntityLoader<JobOffer> GetLoader()
        {
            return new OfferQueryLoader();
        }
    }
}
