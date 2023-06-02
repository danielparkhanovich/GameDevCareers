using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class ApplicationQueryLoader : IEntityLoader<OfferApplication>
    {
        public IQueryable<OfferApplication> Load(IQueryable<OfferApplication> queryable)
        {
            var applications = queryable.Include(application => application.EmployeeProfile);
            return applications;
        }
    }
}
