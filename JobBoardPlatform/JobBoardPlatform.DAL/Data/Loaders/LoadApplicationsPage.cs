using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class LoadApplicationsPage : ILoader<List<OfferApplication>>
    {
        private readonly IQueryable<OfferApplication> applicationsQuery;


        public LoadApplicationsPage(IQueryable<OfferApplication> applicationsQuery)
        {
            this.applicationsQuery = applicationsQuery;
        }

        public async Task<List<OfferApplication>> Load()
        {
            var applications = await applicationsQuery
                .Include(application => application.EmployeeProfile)
                .ToListAsync();

            return applications;
        }
    }
}
