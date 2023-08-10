using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.DAL.Repositories.Cache.Tokens
{
    public class CompanyRegistrationConfirmationTokensCacheRepository : CacheRepositoryCore<ConfirmationToken>
    {
        public const int CacheExpirationTimeInMinutes = 120;


        public CompanyRegistrationConfirmationTokensCacheRepository(IDistributedCache cache) : base(cache)
        {

        }

        protected override DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
        }
    }
}
