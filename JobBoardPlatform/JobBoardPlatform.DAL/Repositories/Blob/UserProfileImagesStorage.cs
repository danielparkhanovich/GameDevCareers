using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using JobBoardPlatform.DAL.Repositories.Blob.Settings;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob
{
    public class UserProfileImagesStorage : IUserProfileImagesStorage
    {
        private readonly string containerName;
        private readonly CoreBlobStorage blobStorage;
        private readonly BlobHttpHeaders blobHttpHeaders;


        public UserProfileImagesStorage(CoreBlobStorage blobStorage, IBlobStorageSettings storageSettings)
        {
            this.blobStorage = blobStorage;
            this.containerName = storageSettings.GetContainerName(this.GetType());

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

        public Task<bool> IsImageExistsAsync(string? path)
        {
            return blobStorage.IsExistsAsync(path, containerName);
        }

        private BlobExportData GetExportData(IFormFile file)
        {
            return new BlobExportData()
            {
                File = file,
                BlobHttpHeaders = blobHttpHeaders,
                Metadata = new Dictionary<string, string>()
            };
        }
    }
}
