using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobBoardPlatform.BLL.Services.Background
{
    public class ExpiredOffersBackgroundCleaner : BackgroundService
    {
        private const int CheckDelayTimeInDays = 1;

        private readonly IServiceScopeFactory serviceScopeFactory;


        public ExpiredOffersBackgroundCleaner(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await TrySetOffersExpirationState();
                await Task.Delay(TimeSpan.FromDays(CheckDelayTimeInDays));
            }
        }

        private async Task TrySetOffersExpirationState()
        {
            using var scope = serviceScopeFactory.CreateScope();
            await ProcessScopeTask(scope);
        }

        private async Task ProcessScopeTask(IServiceScope scope)
        {
            var offersRepository = GetRepository(scope);
            var allOffers = await offersRepository.GetAll();
            foreach (var offer in allOffers)
            {
                if (IsOfferExpired(offer))
                {
                    // possible archieve here...
                    await DeleteOffer(scope.ServiceProvider, offer.Id);
                }
            }
        }

        private IRepository<JobOffer> GetRepository(IServiceScope scope)
        {
            var serviceProvider = scope.ServiceProvider;
            var dataContext = serviceProvider.GetRequiredService<DataContext>();
            return new CoreRepository<JobOffer>(dataContext);
        }

        private bool IsOfferExpired(JobOffer offer)
        {
            var expirationService = new OfferExpirationService(offer);
            return expirationService.IsExpired();
        }

        private Task DeleteOffer(IServiceProvider serviceProvider, int offerIdToDelete)
        {
            var offersManager = serviceProvider.GetRequiredService<IOfferManager>();
            return offersManager.DeleteAsync(offerIdToDelete);
        }
    }
}
