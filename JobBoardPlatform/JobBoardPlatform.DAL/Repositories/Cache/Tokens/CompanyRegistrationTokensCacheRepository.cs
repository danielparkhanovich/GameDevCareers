using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace JobBoardPlatform.DAL.Repositories.Cache.Tokens
{
    public class CompanyRegistrationTokensCacheRepository<T> : CacheRepositoryCore<DataToken<T>>
    {
        private const int CacheExpirationTimeInMinutes = 120;

        private readonly JsonSerializerSettings serializerSettings;


        public CompanyRegistrationTokensCacheRepository(
            IDistributedCache cache, JsonSerializerSettings serializerSettings) : base(cache)
        {
            this.serializerSettings = serializerSettings;
        }

        protected override JsonSerializerSettings GetSerializerSettings()
        {
            return serializerSettings;
        }

        protected override DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
        }
    }
}
