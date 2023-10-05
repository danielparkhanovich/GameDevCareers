using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class OfferWithApplicationsQueryLoader : IEntityLoader<JobOffer>
    {
        public IQueryable<JobOffer> Load(IQueryable<JobOffer> queryable)
        {
            var offer = queryable
                .Include(x => x.OfferApplications)
                    .ThenInclude(x => x.EmployeeProfile);
            return offer;
        }
    }
}
