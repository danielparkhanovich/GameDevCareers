using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class AdminCommandsExecutor
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<CompanyIdentity> companyIdentityRepository;
        private readonly IOffersCacheManager cacheManager;
        private readonly MainPageOffersSearcher offersSearcher;
        private readonly IOffersManager offersManager;


        public AdminCommandsExecutor(
            IRepository<JobOffer> offersRepository,
            IRepository<CompanyIdentity> companyIdentityRepository,
            IOffersCacheManager cacheManager,
            MainPageOffersSearcher offersSearcher,
            IOffersManager offersManager)
        {
            this.offersRepository = offersRepository;
            this.companyIdentityRepository = companyIdentityRepository;
            this.cacheManager = cacheManager;
            this.offersSearcher = offersSearcher;
            this.offersManager = offersManager;
        }

        public async Task GenerateOffers(int companyId, int offersCountToGenerate)
        {
            var command = new GenerateOffersCommand(
                offersCountToGenerate, companyId, companyIdentityRepository, offersRepository);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task DeleteAllOffers()
        {
            var command = new DeleteAllOffersCommand(offersRepository, offersManager);
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
