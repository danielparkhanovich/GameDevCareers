using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.CompanyPanel.Applications;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.PL.Aspects.DataValidators.Offers;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

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
            AddValidatorServices(services);
        }

        private static void AddManagerServices(IServiceCollection services)
        {
            services.AddTransient<IOfferManager, OfferManager>();
            services.AddTransient<IOfferCacheManager, OfferCacheManager>();
            services.AddTransient<IOfferQueryExecutor, OfferQueryExecutor>();
            services.AddTransient<IOfferPlanQueryExecutor, OfferPlanQueryExecutor>();
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
            services.AddTransient<IApplicationsManager, ApplicationsManager>();
            services.AddTransient<OfferApplicationsSearcher>();
            services.AddTransient<IPageSearchParamsUrlFactory<CompanyPanelApplicationSearchParams>, CompanyPanelApplicationSearchParamsFactory>();
        }

        private static void AddValidatorServices(IServiceCollection services)
        {
            services.AddScoped<IValidator<OfferApplicationUpdateViewModel>, OfferApplicationFormValidator>();
            services.AddScoped<IValidator<AttachedResume>, ResumeValidator>();
        }
    }
}
