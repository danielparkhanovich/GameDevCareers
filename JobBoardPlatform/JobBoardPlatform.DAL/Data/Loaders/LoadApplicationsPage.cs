using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class LoadApplicationsPage : ILoader<List<OfferApplication>>
    {
        private readonly IRepository<OfferApplication> repository;
        private readonly int offerId;

        private readonly int page;
        private readonly int pageSize;
        private readonly bool[] flagState;
        private readonly SortType sortType;
        private readonly SortCategoryType sortCategory;

        public int SelectedApplicationsCount { get; private set; }


        public LoadApplicationsPage(IRepository<OfferApplication> repository,
            int offerId, 
            int page, 
            int pageSize, 
            bool[] flagState, 
            SortType sortType, 
            SortCategoryType sortCategory)
        {
            this.repository = repository;
            this.offerId = offerId;
            this.page = page;
            this.pageSize = pageSize;
            this.flagState = flagState;
            this.sortType = sortType;
            this.sortCategory = sortCategory;
        }

        public async Task<List<OfferApplication>> Load()
        {
            var applicationsSet = await repository.GetAllSet();

            var companyApplications = applicationsSet.Where(application => application.JobOfferId == offerId);

            var selectedApplications = companyApplications.Where(application =>
                 (application.ApplicationFlagTypeId == 1 && flagState[0]) ||
                 (application.ApplicationFlagTypeId == 2 && flagState[1]) ||
                 (application.ApplicationFlagTypeId == 3 && flagState[2]) ||
                 (application.ApplicationFlagTypeId == 4 && flagState[3]));

            this.SelectedApplicationsCount = selectedApplications.Count();

            selectedApplications = GetSorted(selectedApplications);

            var pageApplications = await selectedApplications
                .Include(application => application.EmployeeProfile)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return pageApplications;
        }

        private IQueryable<OfferApplication> GetSorted(IQueryable<OfferApplication> query)
        {
            if (sortCategory == SortCategoryType.PublishDate)
            {
                query = query.OrderBy(x => x.CreatedAt);
            }
            else if (sortCategory == SortCategoryType.Alphabetically)
            {
                query = query.OrderBy(x => x.FullName);
            }
            else if (sortCategory == SortCategoryType.Relevenacy)
            {
                query = query.OrderBy(x =>
                    x.ApplicationFlagTypeId == 1 ? 0 :
                    x.ApplicationFlagTypeId == 2 ? 3 :
                    x.ApplicationFlagTypeId == 3 ? 2 :
                    1);
            }

            if (sortType == SortType.Descending)
            {
                query = query.Reverse();
            }

            return query;
        }
    }
}
