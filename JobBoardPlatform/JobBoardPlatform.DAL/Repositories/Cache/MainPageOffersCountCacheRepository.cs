using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.DAL.Repositories.Cache
{
    public class MainPageOffersCountCacheRepository : CacheRepositoryCore<int>
    {
        private const int CacheExpirationTimeInMinutes = 60;


        public MainPageOffersCountCacheRepository(IDistributedCache cache) : base(cache)
        {

        }

        protected override DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
        }
    }
}
