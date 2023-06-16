using JobBoardPlatform.DAL.Models.Company;
using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.DAL.Repositories.Cache
{
    public class MainPageOffersCacheRepository : CacheRepositoryCore<List<JobOffer>>
    {
        private const int CacheExpirationTimeInMinutes = 5;


        public MainPageOffersCacheRepository(IDistributedCache cache) : base(cache)
        {

        }

        protected override DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
        }
    }
}
