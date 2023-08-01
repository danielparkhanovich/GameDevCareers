using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OfferCacheManager : IOfferCacheManager
    {
        private const string MainPageOffersEntryKey = "MainPageOffers";
        private const string MainPageOffersCountEntryKey = "MainPageOffersCount";

        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;


        public OfferCacheManager(
            ICacheRepository<List<JobOffer>> offersCache, 
            ICacheRepository<int> offersCountCache)
        {
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        public async Task<EntitiesFilteringSearchResponse<JobOffer>> GetOffersFromCache()
        {
            var offers = await offersCache.GetAsync(MainPageOffersEntryKey);
            int totalRecordsAfterFilters = await offersCountCache.GetAsync(MainPageOffersCountEntryKey);
            return new EntitiesFilteringSearchResponse<JobOffer>()
            {
                TotalRecordsAfterFilters = totalRecordsAfterFilters,
                Entities = offers
            };
        }

        public async Task UpdateCache(EntitiesFilteringSearchResponse<JobOffer> searchResponse)
        {
            var entities = searchResponse.Entities;
            int totalRecordsAfterFilters = searchResponse.TotalRecordsAfterFilters;

            await offersCache.UpdateAsync(MainPageOffersEntryKey, entities);
            await offersCountCache.UpdateAsync(MainPageOffersCountEntryKey, totalRecordsAfterFilters);
        }
    }
}
