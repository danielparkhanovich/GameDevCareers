using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Company;
using Microsoft.Extensions.DependencyInjection;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob.Settings;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using JobBoardPlatform.DAL.Contexts;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.Services
{
    internal static class TestSetup
    {
        public static void AddMockServices(IServiceCollection services)
        {
            services.AddTransient<IOfferCacheManager, OffersCacheManagerMock>();
            services.AddTransient<IOfferQueryExecutor, OfferQueryExecutorMock>();
            services.AddTransient<IEmailContent<JobOfferApplication>, EmailViewRendererMock>();
            services.AddTransient<IEmailSender, EmailSenderMock>();
        }

        public static void AddSqlRepository(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase(GetUniqueName("IntegrationTestSqlDatabase"));
            });
            services.AddTransient(typeof(IRepository<>), typeof(CoreRepository<>));
            services.AddTransient<OfferContext>();
        }

        public static void AddAzureStorages(IServiceCollection services)
        {
            services.Configure<AzureOptions>(options =>
            {
                GetLocalStorageOptions(options);
            });
            services.AddTransient<CoreBlobStorage>();
            services.AddScoped<IBlobStorageSettings, BlobStorageSettingsMock>();
            services.AddTransient<IUserProfileImagesStorage, UserProfileImagesStorage>();
            services.AddTransient<IApplicationsResumeBlobStorage, UserApplicationsResumeStorage>();
            services.AddTransient<IProfileResumeBlobStorage, UserProfileAttachedResumeStorage>();
        }

        /// <summary>
        /// Used for creating isolated sql databases
        /// and blob storages during parallel tests execution
        /// </summary>
        public static string GetUniqueName(string name)
        {
            return $"{Guid.NewGuid()}{name}";
        }

        private static void GetLocalStorageOptions(AzureOptions options)
        {
            options.Account = "devstoreaccount1";
            options.ResourceGroup = "Test";
            options.ConnectionString = GetStorageEmulatorConnectionString();
        }

        public static string GetStorageEmulatorConnectionString()
        {
            return "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";
        }

        public static void DisposeAllData(IServiceProvider serviceProvider)
        {
            var blobServiceClient = new BlobServiceClient(TestSetup.GetStorageEmulatorConnectionString());
            var storageSettings = serviceProvider.GetService<IBlobStorageSettings>()!;
            DisposeContainer(blobServiceClient, storageSettings.GetContainerName(typeof(UserProfileImagesStorage)));
            DisposeContainer(blobServiceClient, storageSettings.GetContainerName(typeof(UserApplicationsResumeStorage)));
            DisposeContainer(blobServiceClient, storageSettings.GetContainerName(typeof(UserProfileAttachedResumeStorage)));
            DisposeSqlRepository(serviceProvider);
        }

        private static void DisposeContainer(BlobServiceClient client, string containerName)
        {
            var container = client.GetBlobContainerClient(containerName);
            container.DeleteIfExists();
        }

        private static void DisposeSqlRepository(IServiceProvider serviceProvider)
        {
            var dataContext = serviceProvider.GetService<DataContext>()!;
            var dbSetProperties = dataContext.GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var property in dbSetProperties)
            {
                var dbSet = (IEnumerable<object>)property.GetValue(dataContext);
                if (dbSet == null)
                {
                    return;
                }
                dataContext.RemoveRange(dbSet);
            }
            dataContext.SaveChanges();
        }
    }
}
