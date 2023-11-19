using Azure.Core;
using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.BLL.Search.Templates;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Repositories.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

                available = available.Include(offer => offer.EmploymentDetails)
                    .ThenInclude(details => details.SalaryRange)
                    .Where(offer => offer.EmploymentDetails.Any(x => x.SalaryRange != null));
            }
            if (searchParams.MainTechnology != MainPageOfferSearchParams.AllTechnologiesIndex)
            {
                available = available.Where(offer => offer.MainTechnologyTypeId == searchParams.MainTechnology);
            }
            if (!string.IsNullOrEmpty(searchParams.SearchString))
            {
                available = SearchByKeywords(available);
            }

            if (searchParams.Type == OfferType.Employment)
            {
                available = available.Where(x => x.Plan.CategoryId == (int)OfferType.Employment + 1);
            }
            else if (searchParams.Type == OfferType.Commission)
            {
                available = available.Where(x => x.Plan.CategoryId == (int)OfferType.Commission + 1);
            }

            return available;
        }

        protected override IQueryable<JobOffer> GetSorted(IQueryable<JobOffer> available)
        {
            available = available.OrderByDescending(offer => offer.RefreshedOnPageAt);
            return available;
        }

        private IQueryable<JobOffer> SearchByKeywords(IQueryable<JobOffer> available)
        {
            string search = searchParams.SearchString!.Trim();
            string[] keywordsTokens = search.Split().Select(x => x.ToLower())
                .ToArray();

            available = available.Include(offer => offer.CompanyProfile)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.WorkLocation);

            var predicate = PredicateBuilder.New<JobOffer>(false);

            foreach (var keyword in keywordsTokens)
            {
                predicate = predicate.Or(e =>
                    EF.Functions.Like(e.JobTitle, $"%{keyword}%") ||
                    EF.Functions.Like(e.CompanyProfile.CompanyName, $"%{keyword}%") ||
                    EF.Functions.Like(e.WorkLocation.Type, $"%{keyword}%") ||
                    EF.Functions.Like(e.City, $"%{keyword}%") ||
                    EF.Functions.Like(e.Country, $"%{keyword}%") ||
                    EF.Functions.Like(e.MainTechnologyType.Type, $"%{keyword}%") ||
                    e.TechKeywords.Any(t => EF.Functions.Like(t.Name, $"%{keyword}%")));
            }

            available = available.Where(predicate);

            return available;
        }

        protected override IEntityLoader<JobOffer> GetLoader()
        {
            return new OfferQueryLoader();
        }
    }
}
