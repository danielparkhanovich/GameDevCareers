using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Managers;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OfferManager : IOfferManager
    {
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IOfferCacheManager cacheManager;
        private readonly MainPageOffersSearcher offersSearcher;
        private readonly OfferModelData offerModel;


        public OfferManager(
            IOfferQueryExecutor queryExecutor,
            IOfferCacheManager cacheManager,
            MainPageOffersSearcher offersSearcher,
            OfferModelData offerModel)
        {
            this.queryExecutor = queryExecutor;
            this.cacheManager = cacheManager;
            this.offersSearcher = offersSearcher;
            this.offerModel = offerModel;
        }

        public async Task<List<int>> GetAllIdsAsync(int profileId)
        {
            var offers = (await offerModel.OffersRepository.GetAll()).Where(x => x.CompanyProfileId == profileId);
            return offers.Select(x => x.Id).ToList();
        }

        public async Task<JobOffer> GetAsync(int offerId)
        {
            return await queryExecutor.GetOfferById(offerId);
        }

        public async Task AddAsync(int profileId, IOfferData offerData)
        {
            var command = new AddOfferCommand(profileId, offerData, offerModel.OffersRepository);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task UpdateAsync(IOfferData offerData)
        {
            var command = new UpdateOfferCommand(offerData, offerModel);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task DeleteAsync(int offerId)
        {
            var command = new DeleteOfferCommand(queryExecutor, offerModel, offerId);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task ShelveAsync(int offerId, bool flag)
        {
            var command = new ShelveOfferCommand(offerModel.OffersRepository, offerId, flag);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task SuspendAsync(int offerId, bool flag)
        {
            var command = new SuspendOfferCommand(offerModel.OffersRepository, offerId, flag);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task PassPaymentAsync(int offerId)
        {
            var command = new PassPaymentOfferCommand(offerModel.OffersRepository, offerId);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        private async Task ExecuteCommandAndUpdateCacheAsync(ICommand command)
        {
            await command.Execute();
            await UpdateCacheAsync();
        }

        private async Task UpdateCacheAsync()
        {
            var mainPageParams = new MainPageOfferSearchParams();
            var searchResponse = await offersSearcher.Search(mainPageParams);
            await cacheManager.UpdateCache(searchResponse);
        }
    }
}
