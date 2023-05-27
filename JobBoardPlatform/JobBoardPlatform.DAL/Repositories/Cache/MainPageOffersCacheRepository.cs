using JobBoardPlatform.DAL.Models.Company;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
namespace JobBoardPlatform.DAL.Repositories.Cache
{
    public class MainPageOffersCacheRepository : CacheRepositoryCore<List<JobOffer>>
    {
        private const string OffersKey = "MainPageOffers";
        private const int CacheExpirationTimeInMinutes = 5;


        public MainPageOffersCacheRepository(IDistributedCache cache) : base(cache)
        {

        }

        protected override string GetEntryKey()
        {
            return OffersKey;
        }

        protected override DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
        }
    }
}
