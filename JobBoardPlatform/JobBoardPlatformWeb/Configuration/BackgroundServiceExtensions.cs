
using JobBoardPlatform.BLL.Services.Background;

namespace JobBoardPlatform.PL.Configuration
{
    public static class BackgroundServiceExtensions
    {
        public static void AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<ExpiredOffersBackgroundCleaner>();
            services.AddHostedService<OfferBumpUpBackgroundService>();
            services.AddHostedService<TemporaryDataBackgroundCleaner>();
        }
    }
}
