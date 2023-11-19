using Microsoft.Extensions.Hosting;
using JobBoardPlatform.BLL.Generators;
using JobBoardPlatform.BLL.Commands.Offer;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.BLL.Services.Background
{
    public class ShowcaseSnapshotBackgroundService : BackgroundService
    {
        private const int WipeDelayTimeInDays = 30;

        private readonly IServiceScopeFactory serviceScopeFactory;


        public ShowcaseSnapshotBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await WipeAllData();
                await InitData();
                for (int i = 0; i < WipeDelayTimeInDays; i++)
                {
                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

                    var generator = GetGenerator();
                    await generator.CreateOffersForEachCompany(1);
                    if (i % 5 == 0)
                    {
                        await generator.CreateApplications();
                    }
                }
            }
        }

        private async Task WipeAllData()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var manager = serviceProvider.GetRequiredService<AdminCommands>();

            await manager.DeleteAllUsers();
        }

        private async Task InitData()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var generator = serviceProvider.GetRequiredService<IShowcaseSnapshotGenerator>();

            await generator.CreateAdmins();
            await generator.CreateCompanies();
            await generator.CreateOffersForEachCompany(10);
            await generator.CreateEmployees();
            await generator.CreateApplications();
        }

        private IShowcaseSnapshotGenerator GetGenerator()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            return serviceProvider.GetRequiredService<IShowcaseSnapshotGenerator>();
        }
    }
}
