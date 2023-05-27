using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OfferUpdateCacheCommand : ICommand
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;


        public OfferUpdateCacheCommand(IRepository<JobOffer> offersRepository,
            ICacheRepository<List<JobOffer>> offersCache,
            ICacheRepository<int> offersCountCache)
        {
            this.offersRepository = offersRepository;
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        public async Task Execute()
        {
            var searcher = new MainPageOffersSearcher();
            await offersCache.UpdateAsync(await searcher.Search(offersRepository));
            await offersCountCache.UpdateAsync(searcher.AfterFiltersCount);
        }
    }
}
