using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace JobBoardPlatform.BLL.Search.Offers
{
    /// <summary>
    /// Searcher with destributed cache on the top
    /// </summary>
    public class SearchActualOffersCache : ISearcher<List<JobOffer>>
    {
        private const string OffersKey = "MainPageOffers";
        private const string CountKey = "MainPageOffersCount";
        private const int CacheExpirationTimeInMinutes = 5;

        private readonly IDistributedCache distributedCache;
        private readonly SearchActualOffers searcher;
        private readonly OfferSearchData searchData;

        public int AfterFiltersCount { get; set; }


        public SearchActualOffersCache(IRepository<JobOffer> repository, 
            OfferSearchData searchData,
            IDistributedCache distributedCache,
            int pageSize) 
        {
            this.searchData = searchData;
            this.distributedCache = distributedCache;
            this.searcher = new SearchActualOffers(repository, searchData, pageSize);
        }

        public async Task<List<JobOffer>> Search()
        {
            List<JobOffer> offers = new List<JobOffer>();

            if (searchData.IsQueryParams)
            {
                offers = await searcher.Search();
                AfterFiltersCount = searcher.AfterFiltersCount;
                return offers;
            }

            var cacheEntry = await distributedCache.GetAsync(OffersKey);
            if (cacheEntry == null)
            {
                offers = await searcher.Search();
                AfterFiltersCount = searcher.AfterFiltersCount;

                await UpdateCache(offers);

                return offers;
            }

            var countEntry = await distributedCache.GetStringAsync(CountKey);
            AfterFiltersCount = int.Parse(countEntry);

            var serializedCacheEntry = Encoding.UTF8.GetString(cacheEntry);
            offers = JsonConvert.DeserializeObject<List<JobOffer>>(serializedCacheEntry);

            return offers;
        }

        private async Task UpdateCache(List<JobOffer> offers)
        {
            var serializedRecords = JsonConvert.SerializeObject(offers);
            var serializedInt = JsonConvert.SerializeObject(AfterFiltersCount);

            var encodedRecords = Encoding.UTF8.GetBytes(serializedRecords);
            var encodedInt = Encoding.UTF8.GetBytes(serializedInt);

            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));

            if (encodedRecords.Length == 0)
            {
                throw new Exception("Serialization error");
            }

            await distributedCache.SetAsync(OffersKey, encodedRecords, options);
            await distributedCache.SetAsync(CountKey, encodedInt, options);
        }
    }
}
