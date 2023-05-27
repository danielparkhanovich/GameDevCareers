using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Models;
using System.ComponentModel.Design;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class OfferCommandsExecutor
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<TechKeyword> keywordsRepository;
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;


        public OfferCommandsExecutor(IRepository<JobOffer> offersRepository,
            IRepository<TechKeyword> keywordsRepository,
            ICacheRepository<List<JobOffer>> offersCache,
            ICacheRepository<int> offersCountCache)
        {
            this.offersRepository = offersRepository;
            this.keywordsRepository = keywordsRepository;
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        public async Task AddAsync(int profileId, INewOfferData offerData)
        {
            var command = new AddNewOfferCommand(profileId, offerData, keywordsRepository, offersRepository);
            await ExecuteCommandAndUpdateCache(command);
        }

        public async Task DeleteAsync(int offerId)
        {
            var command = new DeleteOfferCommand(offersRepository, offerId);
            await ExecuteCommandAndUpdateCache(command);
        }

        public async Task ShelveAsync(int offerId, bool flag)
        {
            var command = new ShelveOfferCommand(offersRepository, offerId, flag);
            await ExecuteCommandAndUpdateCache(command);
        }

        public async Task SuspendAsync(int offerId, bool flag)
        {
            var command = new SuspendOfferCommand(offersRepository, offerId, flag);
            await ExecuteCommandAndUpdateCache(command);
        }

        public async Task PassPaymentAsync(int offerId)
        {
            var command = new PassPaymentOfferCommand(offersRepository, offerId);
            await ExecuteCommandAndUpdateCache(command);
        }

        private async Task ExecuteCommandAndUpdateCache(ICommand command)
        {
            await command.Execute();
            await UpdateCache();
        }

        private async Task UpdateCache()
        {
            var command = new OfferUpdateCacheCommand(offersRepository, offersCache, offersCountCache);
            await command.Execute();
        }
    }
}
