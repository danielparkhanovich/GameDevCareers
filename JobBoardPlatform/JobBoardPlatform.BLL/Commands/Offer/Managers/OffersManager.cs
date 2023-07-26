using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OffersManager : IOffersManager
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<JobOfferContactDetails> contactDetailsRepository;
        private readonly IRepository<JobOfferEmploymentDetails> employmentDetailsRepository;
        private readonly IRepository<JobOfferSalariesRange> salariesRangeRepository;
        private readonly IRepository<JobOfferTechKeyword> techKeywordsRepository;
        private readonly IRepository<JobOfferApplication> applicationsRepository;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage applicationsResumeStorage;
        private readonly IOffersCacheManager cacheManager;
        private readonly MainPageOffersSearcher offersSearcher;


        public OffersManager(
            IRepository<JobOffer> offersRepository,
            IRepository<JobOfferContactDetails> contactDetailsRepository,
            IRepository<JobOfferEmploymentDetails> employmentDetailsRepository,
            IRepository<JobOfferSalariesRange> salariesRangeRepository,
            IRepository<JobOfferTechKeyword> techKeywordsRepository,
            IRepository<JobOfferApplication> applicationsRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage applicationsResumeStorage,
            IOffersCacheManager cacheManager,
            MainPageOffersSearcher offersSearcher)
        {
            this.offersRepository = offersRepository;
            this.contactDetailsRepository = contactDetailsRepository;
            this.employmentDetailsRepository = employmentDetailsRepository;
            this.salariesRangeRepository = salariesRangeRepository;
            this.applicationsRepository = applicationsRepository;
            this.techKeywordsRepository = techKeywordsRepository;
            this.profileResumeStorage = profileResumeStorage;
            this.applicationsResumeStorage = applicationsResumeStorage;
            this.cacheManager = cacheManager;
            this.offersSearcher = offersSearcher;
        }

        public async Task<List<int>> GetAsync(int profileId)
        {
            var offers = (await offersRepository.GetAll()).Where(x => x.CompanyProfileId == profileId);
            return offers.Select(x => x.Id).ToList();
        }

        public async Task AddAsync(int profileId, INewOfferData offerData)
        {
            var command = new AddNewOfferCommand(profileId, offerData, offersRepository);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task DeleteAsync(int offerId)
        {
            var command = new DeleteOfferCommand(
                    offersRepository,
                    contactDetailsRepository,
                    employmentDetailsRepository,
                    salariesRangeRepository,
                    techKeywordsRepository,
                    applicationsRepository,
                    profileResumeStorage,
                    applicationsResumeStorage,
                    offerId);
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
            var mainPageParams = new MainPageOfferSearchParams();
            var searchResponse = await offersSearcher.Search(mainPageParams);
            await cacheManager.UpdateCache(searchResponse);
        }
    }
}
