using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.DAL.Repositories.Cache.Tokens
{
    public class RestorePasswordTokensCacheRepository : CacheRepositoryCore<RestorePasswordToken>
    {
        private const int CacheExpirationTimeInMinutes = 60;


        public RestorePasswordTokensCacheRepository(IDistributedCache cache) : base(cache)
        {

        }

        protected override DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
        }
    }
}
