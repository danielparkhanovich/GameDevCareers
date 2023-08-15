using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class OfferPlanQueryLoader : IEntityLoader<JobOfferPlan>
    {
        public IQueryable<JobOfferPlan> Load(IQueryable<JobOfferPlan> queryable)
        {
            var offer = queryable
                .Include(x => x.Category)
                .Include(x => x.Name);
            return offer;
        }
    }
}
