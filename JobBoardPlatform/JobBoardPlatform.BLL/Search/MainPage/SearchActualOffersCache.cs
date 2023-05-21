using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    /// <summary>
    /// Searcher with destributed cache on the top
    /// </summary>
    public class SearchActualOffersCache : ISearcher<JobOffer, MainPageOfferSearchParameters>
    {
        private const string OffersKey = "MainPageOffers";
        private const string CountKey = "MainPageOffersCount";
        private const int CacheExpirationTimeInMinutes = 5;

        private readonly IDistributedCache distributedCache;
        private readonly MainPageOffersSearcher searcher;

        public int AfterFiltersCount { get; set; }
        public MainPageOfferSearchParameters SearchParams { get; set; }


        public SearchActualOffersCache(MainPageOfferSearchParameters searchParams, IDistributedCache distributedCache)
        {
            this.searcher = new MainPageOffersSearcher(searchParams);
            this.SearchParams = searchParams;
            this.distributedCache = distributedCache;
        }

        public async Task<List<JobOffer>> Search(IRepository<JobOffer> repository)
        {
            List<JobOffer> offers = new List<JobOffer>();

            if (SearchParams.IsQueryParams)
            {
                offers = await GetOffersFromRepository(repository);
            }
            else
            {
                offers = await TryGetOffersFromCache(repository);
            }

            return offers;
        }

        private async Task<List<JobOffer>> TryGetOffersFromCache(IRepository<JobOffer> repository)
        {
            List<JobOffer> offers = new List<JobOffer>();

            var cacheEntry = await distributedCache.GetAsync(OffersKey);
            if (IsSerializedBytesNull(cacheEntry))
            {
                offers = await GetOffersFromRepository(repository);
                await UpdateCache(offers);
            }
            else
            {
                offers = await GetOffersFromCache(cacheEntry);
            }

            return offers;
        }

        private bool IsSerializedBytesNull(byte[]? cacheEntry)
        {
            return cacheEntry == null || cacheEntry.Length == 0 || Encoding.UTF8.GetString(cacheEntry) == "[]";
        }

        private async Task<List<JobOffer>> GetOffersFromRepository(IRepository<JobOffer> repository)
        {
            List<JobOffer> offers = await searcher.Search(repository);
            AfterFiltersCount = searcher.AfterFiltersCount;
            return offers;
        }

        private async Task<List<JobOffer>> GetOffersFromCache(byte[]? cacheEntry)
        {
            var countEntry = await distributedCache.GetStringAsync(CountKey);
            AfterFiltersCount = int.Parse(countEntry);

            var serializedCacheEntry = Encoding.UTF8.GetString(cacheEntry);
            List<JobOffer> offers = JsonConvert.DeserializeObject<List<JobOffer>>(serializedCacheEntry);

            if (offers == null || offers.Count == 0)
            {
                throw new Exception("Deserialization error");
            }

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

            if (IsSerializedBytesNull(encodedRecords))
            {
                throw new Exception("Serialization error");
            }

            await distributedCache.SetAsync(OffersKey, encodedRecords, options);
            await distributedCache.SetAsync(CountKey, encodedInt, options);
        }
    }
}
