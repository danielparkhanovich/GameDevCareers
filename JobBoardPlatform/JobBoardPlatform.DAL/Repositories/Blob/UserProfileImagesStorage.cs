using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob
{
    public class UserProfileImagesStorage : IUserProfileImagesStorage
    {
        public const string ContainerName = "userprofileimagescontainer";

        private readonly CoreBlobStorage blobStorage;
        private readonly BlobHttpHeaders blobHttpHeaders;


        public UserProfileImagesStorage(CoreBlobStorage blobStorage)
        {
            this.blobStorage = blobStorage;
            this.blobStorage.SetContainerName(ContainerName);

            this.blobHttpHeaders = new BlobHttpHeaders()
            {
                ContentType = "image/bitmap"
            };
        }

        public async Task<string> ChangeImageAsync(string? path, IFormFile newFile)
        {
            await blobStorage.DeleteIfExistsAsync(path);

            var exportData = GetExportData(newFile);
            return await blobStorage.AddAsync(exportData);
        }

        public Task DeleteImageIfExistsAsync(string? path)
        {
            return blobStorage.DeleteIfExistsAsync(path);
        }

        public Task<bool> IsImageExistsAsync(string? path)
        {
            return blobStorage.IsExistsAsync(path);
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
