using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Net;

namespace JobBoardPlatform.DAL.Repositories.Blob
{
    public abstract class CoreBlobStorage : IBlobStorage
    {
        private const string PropertiesSeparator = "_";
        private const string NameProperty = "Name";

        private readonly BlobServiceClient blobServiceClient;


        public CoreBlobStorage(IOptions<AzureOptions> azureOptions)
        {
            blobServiceClient = new BlobServiceClient(azureOptions.Value.ConnectionString);
        }

        /// <returns>Path to the file</returns>
        public async Task<string> AddAsync(IFormFile file)
        {
            BlobContainerClient containerClient = await GetContainerClientAsync();

            string fullFileName = $"{Guid.NewGuid()}{PropertiesSeparator}{file.FileName}";

            BlobClient blobClient = containerClient.GetBlobClient(fullFileName);

            var metaData = new Dictionary<string, string>();
            metaData[NameProperty] = file.FileName;

            using (var fileUploadStream = new MemoryStream())
            {
                file.CopyTo(fileUploadStream);
                fileUploadStream.Position = 0;
                await blobClient.UploadAsync(fileUploadStream, new BlobUploadOptions()
                {
                    HttpHeaders = GetBlobHttpHeaders(),
                    Metadata = metaData
                }, cancellationToken: default);
            }

            return WebUtility.UrlDecode(blobClient.Uri.ToString());
        }

        public async Task<string> UpdateAsync(string? path, IFormFile newFile)
        {
            if (!path.IsNullOrEmpty())
            {
                await DeleteAsync(path);
            }

            var newPath = await AddAsync(newFile);
            return newPath;
        }

        public async Task DeleteAsync(string path)
        {
            BlobContainerClient containerClient = await GetContainerClientAsync();

            var fileName = path.Split('/').Last();
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            bool isExists = await blobClient.ExistsAsync();
            if (!isExists)
            {
                return;
            }

            await blobClient.DeleteAsync();
        }

        public async Task<string> GetBlobName(string path)
        {
            var blobProperties = await GetBlobProperties(path);

            string fileName = string.Empty;

            if (blobProperties.Metadata.ContainsKey(NameProperty)) 
            {
                fileName = blobProperties.Metadata[NameProperty];
            }

            return fileName.ToString();
        }

        public async Task<string> GetBlobSize(string path)
        {
            var blobProperties = await GetBlobProperties(path);

            long fileSizeInBytes = blobProperties.ContentLength;

            float fileSize = (float)fileSizeInBytes / 1024;
            string format = "Kb";

            if (fileSize > 1024)
            {
                fileSize /= 1024;
                format = "Mb";
            }

            return $"{fileSize.ToString("#.##")} {format}";
        }

        protected abstract string GetContainerName();

        protected abstract BlobHttpHeaders GetBlobHttpHeaders();

        private async Task<BlobContainerClient> GetContainerClientAsync()
        {
            string containerName = GetContainerName();

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.Blob);
            }
            return containerClient;
        }

        private async Task<BlobProperties> GetBlobProperties(string path)
        {
            BlobContainerClient containerClient = await GetContainerClientAsync();

            var fileName = path.Split('/').Last();
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            var properties = await blobClient.GetPropertiesAsync();
            return properties;
        }
    }
}
