using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using JobBoardPlatform.DAL.Repositories.Blob.Settings;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob.Temporary
{
    public class UserProfileImagesTemporaryStorage : IUserProfileImagesTemporaryStorage
    {
        public const string ExpirationPropertyName = "ExpirationTime";

        private readonly string containerName;
        private readonly CoreBlobStorage blobStorage;
        private readonly BlobHttpHeaders blobHttpHeaders;


        public UserProfileImagesTemporaryStorage(CoreBlobStorage blobStorage, IBlobStorageSettings storageSettings)
        {
            this.blobStorage = blobStorage;
            this.containerName = storageSettings.GetContainerName(typeof(UserProfileImagesStorage));

            this.blobHttpHeaders = new BlobHttpHeaders()
            {
                ContentType = "image/bitmap"
            };
        }

        public async Task<string> ChangeImageAsync(string? path, IFormFile newFile)
        {
            await blobStorage.DeleteIfExistsAsync(path, containerName);

            var exportData = GetExportData(newFile);
            return await blobStorage.AddAsync(exportData, containerName);
        }

        public Task DeleteImageIfExistsAsync(string? path)
        {
            return blobStorage.DeleteIfExistsAsync(path, containerName);
        }

        private BlobExportData GetExportData(IFormFile file)
        {
            return new BlobExportData()
            {
                File = file,
                BlobHttpHeaders = blobHttpHeaders,
                Metadata = GetMetadata()
            };
        }

        private Dictionary<string, string> GetMetadata()
        {
            return new Dictionary<string, string>()
            {
                { ExpirationPropertyName, GetExpirationTimeString() }
            };
        }

        private string GetExpirationTimeString()
        {
            int expirationInMinutes = CompanyRegistrationConfirmationTokensCacheRepository.CacheExpirationTimeInMinutes;
            var expirationTimeSpan = TimeSpan.FromMinutes(expirationInMinutes);
            return (DateTimeOffset.UtcNow + expirationTimeSpan).ToString("o");
        }
    }
}
