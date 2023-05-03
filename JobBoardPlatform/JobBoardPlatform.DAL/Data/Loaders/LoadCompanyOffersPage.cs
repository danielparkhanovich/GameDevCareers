using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class LoadCompanyOffersPage : ILoader<List<JobOffer>>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly int profileId;

        private readonly int page;
        private readonly int pageSize;
        private readonly bool[] flagState;
        private readonly SortType sortType;
        private readonly SortCategoryType sortCategory;

        public int SelectedCount { get; private set; }


        public LoadCompanyOffersPage(IRepository<JobOffer> repository,
            int companyProfileId,
            int page,
            int pageSize,
            bool[] flagState,
            SortType sortType,
            SortCategoryType sortCategory)
        {
            this.repository = repository;
            this.profileId = companyProfileId;

            this.page = page;
            this.pageSize = pageSize;
            this.flagState = flagState;
            this.sortType = sortType;
            this.sortCategory = sortCategory;
        }

        public async Task<List<JobOffer>> Load()
        {
            var offersSet = await repository.GetAllSet();

            var companyOffers = offersSet.Where(offer => offer.CompanyProfileId == profileId);

            var selectedOffers = companyOffers.Where(offer =>
                 (offer.IsPublished && !offer.IsShelved && !offer.IsDeleted && !offer.IsSuspended && flagState[0]) ||
                 ((offer.IsShelved || offer.IsDeleted || offer.IsSuspended || !offer.IsPaid) && flagState[1]));

            selectedOffers = GetSorted(selectedOffers);

            SelectedCount = selectedOffers.Count();

            var pageOffers = await selectedOffers
                .Include(offer => offer.CompanyProfile)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .Include(offer => offer.ContactDetails)
                    .ThenInclude(details => details.ContactType)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return pageOffers;
        }

        private IQueryable<JobOffer> GetSorted(IQueryable<JobOffer> query)
        {
            if (sortCategory == SortCategoryType.PublishDate)
            {
                query = query.OrderBy(x => x.CreatedAt);
            }
            else if (sortCategory == SortCategoryType.Alphabetically)
            {
                query = query.OrderBy(x => x.JobTitle);
            }
            else if (sortCategory == SortCategoryType.Relevenacy)
            {
                query = query.OrderBy(x => x.NumberOfViews + (x.NumberOfApplications * 2));
            }

            if (sortType == SortType.Descending)
            {
                query = query.Reverse();
            }

            return query;
        }
    }
}
