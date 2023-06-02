using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OfferCommandsExecutor
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<TechKeyword> keywordsRepository;
        private readonly OffersCacheManager cacheManager;
        private readonly MainPageOffersSearcher offersSearcher;


        public OfferCommandsExecutor(IRepository<JobOffer> offersRepository,
            IRepository<TechKeyword> keywordsRepository,
            OffersCacheManager cacheManager,
            MainPageOffersSearcher offersSearcher)
        {
            this.offersRepository = offersRepository;
            this.keywordsRepository = keywordsRepository;
            this.cacheManager = cacheManager;
            this.offersSearcher = offersSearcher;
        }

        public async Task AddAsync(int profileId, INewOfferData offerData)
        {
            var command = new AddNewOfferCommand(profileId, offerData, keywordsRepository, offersRepository);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task DeleteAsync(int offerId)
        {
            var command = new DeleteOfferCommand(offersRepository, offerId);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task ShelveAsync(int offerId, bool flag)
        {
            var command = new ShelveOfferCommand(offersRepository, offerId, flag);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task SuspendAsync(int offerId, bool flag)
        {
            var command = new SuspendOfferCommand(offersRepository, offerId, flag);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task PassPaymentAsync(int offerId)
        {
            var command = new PassPaymentOfferCommand(offersRepository, offerId);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        private async Task ExecuteCommandAndUpdateCacheAsync(ICommand command)
        {
            await command.Execute();
            await UpdateCacheAsync();
        }

        private async Task UpdateCacheAsync()
        {
            var searchResponse = await offersSearcher.Search();
            await cacheManager.UpdateCache(searchResponse);
        }
    }
}
