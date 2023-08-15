using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobBoardPlatform.BLL.Services.Background
{
    public class OfferBumpUpBackgroundService : BackgroundService
    {
        private const int CheckDelayTimeInHours = 1;

        private readonly IServiceScopeFactory serviceScopeFactory;


        public OfferBumpUpBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await BumpUpOffersAsync();
                await Task.Delay(TimeSpan.FromHours(CheckDelayTimeInHours), stoppingToken);
            }
        }

        private async Task BumpUpOffersAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            await ProcessScopeTaskAsync(scope);
        }

        private async Task ProcessScopeTaskAsync(IServiceScope scope)
        {
            var loaded = await GetAllOffersAsync(scope);
            var offersToBumpUp = GetOffersToBumpUpAsync(loaded);
            await BumpUpOffersAsync(scope, offersToBumpUp);
        }

        private async Task<List<JobOffer>> GetAllOffersAsync(IServiceScope scope)
        {
            var offersRepository = GetRepository(scope);
            var allOffers = await offersRepository.GetAllSet();

            var loader = new JobOfferWithPlanQueryLoader();
            return await loader.Load(allOffers).ToListAsync();
        }

        private IRepository<JobOffer> GetRepository(IServiceScope scope)
        {
            var serviceProvider = scope.ServiceProvider;
            var repository = serviceProvider.GetRequiredService<IRepository<JobOffer>>();
            return repository;
        }

        private List<JobOffer> GetOffersToBumpUpAsync(List<JobOffer> allOffers)
        {
            var offersToBumpUp = new List<JobOffer>();
            foreach (var offer in allOffers)
            {
                if (IsBumpUpRequired(offer))
                {
                    offersToBumpUp.Add(offer);
                }
            }
            return offersToBumpUp;
        }

        private bool IsBumpUpRequired(JobOffer offer)
        {
            int publishDays = offer.Plan.PublicationDaysCount;
            int totalBumpUps = offer.Plan.OfferRefreshesCount;
            double refreshDeltaInDays = (double)publishDays / totalBumpUps;

            var lastBumpUpAt = offer.RefreshedOnPageAt;
            var nextBumpUpAt = lastBumpUpAt.AddDays(refreshDeltaInDays);
            var currentDate = DateTime.UtcNow;

            return currentDate > nextBumpUpAt;
        }

        private async Task BumpUpOffersAsync(IServiceScope scope, List<JobOffer> offers)
        {
            var repository = GetRepository(scope);

            // set correct bump up order
            offers = offers.OrderBy(x => x.RefreshedOnPageAt).ToList();

            foreach (var offer in offers)
            {
                BumpUpOffer(offer);
                await repository.Update(offer);
                await Task.Delay(TimeSpan.FromMilliseconds(100f));
            }
        }

        private void BumpUpOffer(JobOffer offer)
        {
            offer.RefreshedOnPageAt = DateTime.UtcNow;
        }
    }
}
