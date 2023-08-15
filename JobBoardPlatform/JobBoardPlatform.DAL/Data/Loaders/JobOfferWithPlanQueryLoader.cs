using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class JobOfferWithPlanQueryLoader : IEntityLoader<JobOffer>
    {
        public IQueryable<JobOffer> Load(IQueryable<JobOffer> queryable)
        {
            var offer = queryable
                .Include(x => x.Plan)
                    .ThenInclude(x => x.Name)
                 .Include(x => x.Plan)
                    .ThenInclude(x => x.Category);
            return offer;
        }
    }
}
