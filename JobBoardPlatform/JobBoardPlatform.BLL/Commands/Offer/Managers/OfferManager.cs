using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Contexts;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OfferManager : IOfferManager
    {
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IOfferCacheManager cacheManager;
        private readonly MainPageOffersSearcher offersSearcher;
        private readonly OfferContext offerModel;


        public OfferManager(
            IOfferQueryExecutor queryExecutor,
            IOfferCacheManager cacheManager,
            MainPageOffersSearcher offersSearcher,
            OfferContext offerModel)
        {
            this.queryExecutor = queryExecutor;
            this.cacheManager = cacheManager;
            this.offersSearcher = offersSearcher;
            this.offerModel = offerModel;
        }

        public async Task<List<int>> GetAllIdsAsync()
        {
            var offers = (await offerModel.OffersRepository.GetAll());
            return offers.Select(x => x.Id).ToList();
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

        public async Task AddAsync(int profileId, OfferData offerData)
        {
            var command = new AddOfferCommand(profileId, offerData, offerModel.OffersRepository);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task UpdateAsync(OfferData offerData)
        {
            var command = new UpdateOfferCommand(offerData, this, offerModel);
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
