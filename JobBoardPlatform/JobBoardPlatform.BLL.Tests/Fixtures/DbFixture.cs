using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using JobBoardPlatform.DAL.Models.Company;
using System;
using JobBoardPlatform.DAL.Models.Employee;

namespace JobBoardPlatform.BLL.IntegrationTests.Fixtures
{
    public class DbFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; protected set; }
        protected readonly ServiceCollection serviceCollection;


        public DbFixture()
        {
            serviceCollection = new ServiceCollection();
            AddSqlRepository(serviceCollection);
            AddAzureStorages(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            var blobServiceClient = new BlobServiceClient(GetStorageEmulatorConnectionString());
            DisposeContainer(blobServiceClient, UserProfileImagesStorage.ContainerName);
            DisposeContainer(blobServiceClient, UserApplicationsResumeStorage.ContainerName);
            DisposeContainer(blobServiceClient, UserProfileAttachedResumeStorage.ContainerName);
            DisposeSqlRepository();
        }

        private void AddSqlRepository(ServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestSqlDatabase");
            });
            serviceCollection.AddTransient(typeof(IRepository<>), typeof(CoreRepository<>));
        }

        private void AddAzureStorages(ServiceCollection serviceCollection)
        {
            serviceCollection.Configure<AzureOptions>(options =>
            {
                GetLocalStorageOptions(options);
            });
            serviceCollection.AddTransient<CoreBlobStorage>();
            serviceCollection.AddTransient<IUserProfileImagesStorage, UserProfileImagesStorage>();
            serviceCollection.AddTransient<IApplicationsResumeBlobStorage, UserApplicationsResumeStorage>();
            serviceCollection.AddTransient<IProfileResumeBlobStorage, UserProfileAttachedResumeStorage>();
        }

        private void GetLocalStorageOptions(AzureOptions options)
        {
            options.Account = "devstoreaccount1";
            options.ResourceGroup = "Test";
            options.ConnectionString = GetStorageEmulatorConnectionString();
        }

        private string GetStorageEmulatorConnectionString()
        {
            return "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";
        }

        private void DisposeContainer(BlobServiceClient client, string containerName)
        {
            var container = client.GetBlobContainerClient(containerName);
            container.DeleteIfExists();
        }

        private void DisposeSqlRepository()
        {
            var dataContext = ServiceProvider.GetService<DataContext>()!;
            var dbSetProperties = dataContext.GetType().GetProperties()
            .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var property in dbSetProperties)
            {
                var dbSet = (IEnumerable<object>)property.GetValue(dataContext);
                dataContext.RemoveRange(dbSet);
            }
            dataContext.SaveChanges();
        }
    }
}
