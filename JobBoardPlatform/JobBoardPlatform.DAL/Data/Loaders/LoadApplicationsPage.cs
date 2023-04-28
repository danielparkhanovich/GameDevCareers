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

        public int SelectedApplicationsCount { get; private set; }


        public LoadApplicationsPage(IRepository<OfferApplication> repository,
            int offerId, int page, int pageSize, bool[] flagState)
        {
            this.repository = repository;
            this.offerId = offerId;
            this.page = page;
            this.pageSize = pageSize;
            this.flagState = flagState;
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

            var pageApplications = await selectedApplications
                .Include(application => application.EmployeeProfile)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return pageApplications;
        }
    }
}
