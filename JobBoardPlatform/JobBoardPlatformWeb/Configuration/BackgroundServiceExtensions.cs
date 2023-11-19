using JobBoardPlatform.BLL.Generators;
using JobBoardPlatform.BLL.Services.Background;
using JobBoardPlatform.BLL.Utils;

namespace JobBoardPlatform.PL.Configuration
{
    public static class BackgroundServiceExtensions
    {
        public static void AddBackgroundServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddHostedService<ExpiredOffersBackgroundCleaner>();
                services.AddHostedService<OfferBumpUpBackgroundService>();
                services.AddHostedService<TemporaryDataBackgroundCleaner>();
                services.AddHostedService<TestBackgroundService>();
            }

            if (environment.IsStaging())
            {
                services.AddTransient<UsersGenerator>();
                services.AddTransient<IShowcaseSnapshotGenerator, ShowcaseSnapshotGenerator>();
                services.AddHostedService<ShowcaseSnapshotBackgroundService>();
            }
        }
    }
}
