using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache.Converters;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Company;
using JobBoardPlatform.PL.ViewModels.Models.Registration;
using Newtonsoft.Json;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob.Settings;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.PL.Configuration
{
    public static class DataAccessServiceExtensions
    {
        public static void AddDataAccessServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            AddRepositoryServices(services, configuration);
            AddBlobStorageServices(services, configuration);
            AddCacheServices(services, configuration);
        }

        private static void AddRepositoryServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("JobBoardPlatform.DAL"));
            });
            services.AddTransient(typeof(IRepository<>), typeof(CoreRepository<>));
        }

        private static void AddBlobStorageServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<AzureOptions>(configuration.GetSection("Azure"));
            services.AddTransient<IBlobStorageSettings, BlobStorageSettings>();
            services.AddTransient<CoreBlobStorage>();
            services.AddTransient<IUserProfileImagesStorage, UserProfileImagesStorage>();
            services.AddTransient<IApplicationsResumeBlobStorage, UserApplicationsResumeStorage>();
            services.AddTransient<IProfileResumeBlobStorage, UserProfileAttachedResumeStorage>();
        }

        private static void AddCacheServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("Redis:ConnectionString");
                options.InstanceName = configuration.GetValue<string>("Redis:InstanceName");
            });
            AddJsonSerializerSettings(services);
            services.AddTransient<ICacheRepository<List<JobOffer>>, MainPageOffersCacheRepository>();
            services.AddTransient<ICacheRepository<int>, MainPageOffersCountCacheRepository>();
            services.AddTransient<ICacheRepository<RegistrationToken>, RegistrationTokensCacheRepository>();
            services.AddTransient<ICacheRepository<RestorePasswordToken>, RestorePasswordTokensCacheRepository>();
            services.AddTransient<ICacheRepository<DataToken<ICompanyProfileAndNewOfferData>>, CompanyRegistrationTokensCacheRepository<ICompanyProfileAndNewOfferData>>();
            services.AddTransient<ICacheRepository<ConfirmationToken>, CompanyRegistrationConfirmationTokensCacheRepository>();
        }

        private static void AddJsonSerializerSettings(IServiceCollection services)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>
                {
                    new InterfaceConverter<ICompanyProfileAndNewOfferData, CompanyPublishOfferAndRegisterViewModel>(),
                    new InterfaceConverter<ICompanyProfileData, CompanyProfileViewModel>(),
                    new InterfaceConverter<INewOfferData, OfferDetailsViewModel>(),
                }
            };
            services.AddSingleton<JsonSerializerSettings>(serializerSettings);
        }
    }
}
