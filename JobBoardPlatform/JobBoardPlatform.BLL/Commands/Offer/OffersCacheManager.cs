using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OffersCacheManager
    {
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;


        public OffersCacheManager(
            ICacheRepository<List<JobOffer>> offersCache, 
            ICacheRepository<int> offersCountCache)
        {
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        public async Task<EntitiesFilteringSearchResponse<JobOffer>> GetOffersFromCache()
        {
            var offers = await offersCache.GetAsync();
            int totalRecordsAfterFilters = await offersCountCache.GetAsync();
            return new EntitiesFilteringSearchResponse<JobOffer>()
            {
                TotalRecordsAfterFilters = totalRecordsAfterFilters,
                Entities = offers
            };
        }

        public Task UpdateCache(EntitiesFilteringSearchResponse<JobOffer> searchResponse)
        {
            var entities = searchResponse.Entities;
            int totalRecordsAfterFilters = searchResponse.TotalRecordsAfterFilters;

            var updateCacheCommand = new UpdateOfferCacheCommand(entities, totalRecordsAfterFilters, offersCache, offersCountCache);
            return updateCacheCommand.Execute();
        }
    }
}
