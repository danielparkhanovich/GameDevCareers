using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Query.Identity
{
    public class OfferPlanQueryExecutor : IOfferPlanQueryExecutor
    {
        private readonly IRepository<JobOfferPlan> repository;


        public OfferPlanQueryExecutor(IRepository<JobOfferPlan> repository)
        {
            this.repository = repository;
        }

        public async Task<List<JobOfferPlan>> GetAllAsync()
        {
            var loader = new OfferPlanQueryLoader();
            var plans = await repository.GetAllSet();
            var loaded = loader.Load(plans);
            return await loaded.ToListAsync();
        }
    }
}
