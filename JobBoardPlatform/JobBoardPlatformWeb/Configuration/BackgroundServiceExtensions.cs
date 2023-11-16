using JobBoardPlatform.BLL.Generators;
using JobBoardPlatform.BLL.Services.Background;
using JobBoardPlatform.BLL.Utils;

namespace JobBoardPlatform.PL.Configuration
{
    public static class BackgroundServiceExtensions
    {
        public static void AddBackgroundServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddHostedService<ExpiredOffersBackgroundCleaner>();
            services.AddHostedService<OfferBumpUpBackgroundService>();
            services.AddHostedService<TemporaryDataBackgroundCleaner>();

            if (environment.IsProduction())
            {
                services.AddTransient<UsersGenerator>();
                services.AddTransient<IShowcaseSnapshotGenerator, ShowcaseSnapshotGenerator>();
                services.AddHostedService<ShowcaseSnapshotBackgroundService>();
            }
        }
    }
}
