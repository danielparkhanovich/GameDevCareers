using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class ApplicationQueryLoader : IEntityLoader<JobOfferApplication>
    {
        public IQueryable<JobOfferApplication> Load(IQueryable<JobOfferApplication> queryable)
        {
            var applications = queryable.Include(application => application.EmployeeProfile);
            return applications;
        }
    }
}
