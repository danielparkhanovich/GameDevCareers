using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob.Exceptions;
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;

namespace JobBoardPlatform.DAL.Repositories.Blob
{
    public class CoreBlobStorage
    {
        protected const string PropertiesSeparator = "_";
        private const string NameProperty = "Name";

        private readonly BlobServiceClient blobServiceClient;


        public CoreBlobStorage(IOptions<AzureOptions> azureOptions)
        {
            blobServiceClient = new BlobServiceClient(azureOptions.Value.ConnectionString);
        }

        public async Task<string> AddAsync(BlobExportData exportData, string containerName)
        {
            var file = exportData.File;

            string fileName = GetFileName(file);
            BlobClient blobClient = await GetBlobClientAsync(fileName, containerName);

            var metadata = exportData.Metadata;
            metadata[NameProperty] = file.FileName;

            using (var fileUploadStream = new MemoryStream())
            {
                file.CopyTo(fileUploadStream);
                fileUploadStream.Position = 0;
                await blobClient.UploadAsync(fileUploadStream, new BlobUploadOptions()
                {
                    HttpHeaders = exportData.BlobHttpHeaders,
                    Metadata = metadata
                }, cancellationToken: default);
            }

            return WebUtility.UrlDecode(blobClient.Uri.ToString());
        }

        public async Task DeleteIfExistsAsync(string? filePath, string containerName)
        {
            if (string.IsNullOrEmpty(filePath)) 
            {
                return;
            }

            BlobClient blobClient = await GetBlobClientFromPathAsync(filePath, containerName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> IsExistsAsync(string? filePath, string containerName)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            BlobClient blobClient = await GetBlobClientFromPathAsync(filePath, containerName);
            return await blobClient.ExistsAsync();
        }

        /// <returns>Blob blob metadata or empty object in case if not found</returns>
        public async Task<BlobDescription> TryGetBlobDescriptionAsync(string? resumeUrl, string containerName)
        {
            if (await IsExistsAsync(resumeUrl, containerName))
            {
                return await GetBlobMetadataAsync(resumeUrl!, containerName);
            }
            else
            {
                return new BlobDescription();
            }
        }

        public async Task<IDictionary<string, string>> GetBlobProperties(string path, string containerName)
        {
            return (await TryGetPropertiesFromRequestAsync(path, containerName)).Metadata;
        }

        public async Task SetMetadataAsync(
            string filePath, IDictionary<string, string> metadata, string containerName)
        {
            var blobClient = await GetBlobClientFromPathAsync(filePath, containerName);
            blobClient.SetMetadata(metadata);
        }

        private string GetFileName(IFormFile formFile)
        {
            return $"{Guid.NewGuid()}{PropertiesSeparator}{formFile.FileName}";
        }

        private async Task<BlobDescription> GetBlobMetadataAsync(string filePath, string containerName)
        {
            return new BlobDescription()
            {
                Name = await GetBlobName(filePath, containerName),
                Size = await GetBlobSize(filePath, containerName),
            };
        }

        private async Task<string> GetBlobName(string filePath, string containerName)
        {
            var blobProperties = await GetBlobProperties(filePath, containerName);

            string fileName = string.Empty;
            if (blobProperties.ContainsKey(NameProperty))
            {
                fileName = blobProperties[NameProperty];
            }

            return fileName.ToString();
        }

        private async Task<string> GetBlobSize(string path, string containerName)
        {
            var blobProperties = await TryGetPropertiesFromRequestAsync(path, containerName);
            long fileSizeInBytes = blobProperties.ContentLength;
            return FormatSizeString(fileSizeInBytes);
        }

        private Task<BlobClient> GetBlobClientFromPathAsync(string path, string containerName)
        {
            var fileName = path.Split('/').Last();
            return GetBlobClientAsync(fileName, containerName);
        }

        private async Task<BlobClient> GetBlobClientAsync(string fileName, string containerName)
        {
            BlobContainerClient containerClient = await GetContainerClientOrCreateIfNotExistsAsync(containerName);
            return containerClient.GetBlobClient(fileName);
        }

        private async Task<BlobContainerClient> GetContainerClientOrCreateIfNotExistsAsync(string containerName)
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(
                    containerName, PublicAccessType.Blob);
            }
            return containerClient;
        }

        private async Task<BlobProperties> TryGetPropertiesFromRequestAsync(string path, string containerName)
        {
            try
            {
                BlobClient blobClient = await GetBlobClientFromPathAsync(path, containerName);
                return await blobClient.GetPropertiesAsync();
            }
            catch (RequestFailedException e)
            {
                throw new BlobStorageException(BlobStorageException.ItemNotFound, e);
            }
        }

        private string FormatSizeString(long fileSizeInBytes)
        {
            float fileSize = (float)fileSizeInBytes / 1024;
            string format = "Kb";

            if (fileSize > 1024)
            {
                fileSize /= 1024;
                format = "Mb";
            }

            var fileSizeFormatted = fileSize.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            return $"{fileSizeFormatted} {format}";
        }
    }
}
