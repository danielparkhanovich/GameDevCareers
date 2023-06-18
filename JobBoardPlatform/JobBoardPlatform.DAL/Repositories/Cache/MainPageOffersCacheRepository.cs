using JobBoardPlatform.DAL.Models.Company;
using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.DAL.Repositories.Cache
{
    public class MainPageOffersCacheRepository : CacheRepositoryCore<List<JobOffer>>
    {
        private const int CacheExpirationTimeInMinutes = 60;


        public MainPageOffersCacheRepository(IDistributedCache cache) : base(cache)
        {

        }

        protected override DistributedCacheEntryOptions GetOptions()
        {
            return new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(CacheExpirationTimeInMinutes));
        }
    }
}
