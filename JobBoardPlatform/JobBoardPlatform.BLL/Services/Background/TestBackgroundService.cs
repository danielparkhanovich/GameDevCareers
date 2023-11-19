using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Blob.Settings;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.Temporary;

namespace JobBoardPlatform.BLL.Services.Background
{
    public class TestBackgroundService : BackgroundService
    {
        private const int CheckDelayTimeInDays = 1;

        private readonly IServiceScopeFactory serviceScopeFactory;


        public TestBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _timer = new System.Threading.Timer(Test, null, 0,
                60);
            return Task.CompletedTask;
        }

        private void Test(object? state)
        {
            Console.WriteLine("Test");
        }

        private async Task CleanupExpiredBlobsAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            await ProcessScopeTaskAsync(scope);
        }

        private async Task ProcessScopeTaskAsync(IServiceScope scope)
        {
            var containerClient = GetBlobContainerClient(scope);

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                await DeleteBlobIfExpiredAsync(blobItem, containerClient);
            }
        }

        private BlobContainerClient GetBlobContainerClient(IServiceScope scope)
        {
            var serviceProvider = scope.ServiceProvider;
            var azureOptions = serviceProvider.GetRequiredService<IOptions<AzureOptions>>();
            var blobServiceClient = new BlobServiceClient(azureOptions.Value.ConnectionString);

            var blobSettings = serviceProvider.GetRequiredService<IBlobStorageSettings>();
            string containerName = blobSettings.GetContainerName(typeof(UserProfileImagesStorage));

            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return containerClient;
        }

        private async Task DeleteBlobIfExpiredAsync(BlobItem blobItem, BlobContainerClient containerClient)
        {
            BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
            BlobProperties blobProperties = await blobClient.GetPropertiesAsync();

            if (IsBlobExpired(blobProperties))
            {
                await blobClient.DeleteAsync();
            }
        }

        private bool IsBlobExpired(BlobProperties blobProperties)
        {
            string propertyName = UserProfileImagesTemporaryStorage.ExpirationPropertyName;
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;

            return blobProperties.Metadata.TryGetValue(propertyName, out string expirationTimeString) &&
                   DateTimeOffset.TryParse(expirationTimeString, out DateTimeOffset expirationTime) &&
                   expirationTime <= currentTime;
        }
    }
}
