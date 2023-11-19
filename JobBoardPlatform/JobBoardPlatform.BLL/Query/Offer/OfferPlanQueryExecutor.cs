using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Query.Identity
{
    public class OfferPlanQueryExecutor : IOfferPlanQueryExecutor
    {
        private readonly IRepository<JobOfferPlan> repository;
        private readonly IOfferManager offerManager;


        public OfferPlanQueryExecutor(
            IRepository<JobOfferPlan> repository,
            IOfferManager offerManager)
        {
            this.repository = repository;
            this.offerManager = offerManager;
        }

        public async Task<List<JobOfferPlan>> GetAllAsync()
        {
            var loader = new OfferPlanQueryLoader();
            var plans = await repository.GetAllSet();
            var loaded = loader.Load(plans);
            return await loaded.ToListAsync();
        }

        public async Task<bool> IsFreePlan(int offerId)
        {
            var offer = await offerManager.GetAsync(offerId);
            if (offer.Plan.FreeSlotsCount > 0)
            {
                return true;
            }

            return false;
        }

        public async Task RemoveFreeSlot(int offerId)
        {
            var offer = await offerManager.GetAsync(offerId);

            var plan = await repository.Get(offer.PlanId);
            plan.FreeSlotsCount -= 1;
            await repository.Update(plan);
        }
    }
}
