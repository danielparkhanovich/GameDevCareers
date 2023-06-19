using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;

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
            serviceCollection.AddTransient<UserProfileImagesStorage>();
            serviceCollection.AddTransient<UserProfileAttachedResumeStorage>();
            serviceCollection.AddTransient<UserApplicationsResumeStorage>();
        }

        private void GetLocalStorageOptions(AzureOptions options)
        {
            options.Account = "devstoreaccount1";
            options.ResourceGroup = "Test";
            // Connection string for the emulator
            options.ConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";
        }

        public void Dispose()
        {
            var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;");
            DisposeContainer(blobServiceClient, UserProfileImagesStorage.ContainerName);
            DisposeContainer(blobServiceClient, UserProfileAttachedResumeStorage.ContainerName);
            DisposeContainer(blobServiceClient, UserApplicationsResumeStorage.ContainerName);
        }

        private void DisposeContainer(BlobServiceClient client, string containerName)
        {
            var container = client.GetBlobContainerClient(containerName);
            container.DeleteIfExists();
        }
    }
}
