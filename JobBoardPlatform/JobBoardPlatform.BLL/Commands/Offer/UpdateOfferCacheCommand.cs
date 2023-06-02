using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class UpdateOfferCacheCommand : ICommand
    {
        private readonly List<JobOffer> offers;
        private readonly int totalRecordsAfterFilters;
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;


        public UpdateOfferCacheCommand(List<JobOffer> offers,
            int totalRecordsAfterFilters,
            ICacheRepository<List<JobOffer>> offersCache,
            ICacheRepository<int> offersCountCache)
        {
            this.offers = offers;
            this.totalRecordsAfterFilters = totalRecordsAfterFilters;
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        public async Task Execute()
        {
            await offersCache.UpdateAsync(offers);
            await offersCountCache.UpdateAsync(totalRecordsAfterFilters);
        }
    }
}
