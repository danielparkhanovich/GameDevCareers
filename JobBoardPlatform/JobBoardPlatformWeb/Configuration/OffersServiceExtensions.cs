
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.CompanyPanel.Applications;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.MainPage;

namespace JobBoardPlatform.PL.Configuration
{
    public static class OffersServiceExtensions
    {
        public static void AddOffersServices(this IServiceCollection services)
        {
            AddManagerServices(services);
            AddMainPageServices(services);
            AddCompanyPanelServices(services);
            AddApplicationsServices(services);
        }

        private static void AddManagerServices(IServiceCollection services)
        {
            services.AddTransient<IOfferManager, OfferManager>();
            services.AddTransient<IOfferCacheManager, OfferCacheManager>();
            services.AddTransient<IOfferQueryExecutor, OfferQueryExecutor>();
        }

        private static void AddMainPageServices(IServiceCollection services)
        {
            services.AddTransient<IPageSearchParamsUrlFactory<MainPageOfferSearchParams>, MainPageOfferSearchParamsFactory>();
            services.AddTransient<MainPageOffersSearcher>();
            services.AddTransient<MainPageOffersSearcherCacheDecorator>();
        }

        private static void AddCompanyPanelServices(IServiceCollection services)
        {
            services.AddTransient<IPageSearchParamsUrlFactory<CompanyPanelOfferSearchParameters>, CompanyPanelOfferSearchParametersFactory>();
            services.AddTransient<CompanyOffersSearcher>();
        }

        private static void AddApplicationsServices(IServiceCollection services)
        {
            services.AddTransient<OfferApplicationCommandsExecutor>();
            services.AddTransient<OfferApplicationsSearcher>();
            services.AddTransient<IPageSearchParamsUrlFactory<CompanyPanelApplicationSearchParams>, CompanyPanelApplicationSearchParamsFactory>();
        }
    }
}
