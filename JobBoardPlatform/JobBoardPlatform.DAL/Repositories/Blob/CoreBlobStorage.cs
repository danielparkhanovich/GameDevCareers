using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob.Exceptions;
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
        private string containerName = string.Empty;


        public CoreBlobStorage(IOptions<AzureOptions> azureOptions)
        {
            blobServiceClient = new BlobServiceClient(azureOptions.Value.ConnectionString);
        }

        public void SetContainerName(string containerName)
        {
            this.containerName = containerName;
        }

        public async Task<string> AddAsync(BlobExportData exportData)
        {
            var file = exportData.File;

            string fileName = GetFileName(file);
            BlobClient blobClient = await GetBlobClientAsync(fileName);

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

        public async Task DeleteIfExistsAsync(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath)) 
            {
                return;
            }

            BlobClient blobClient = await GetBlobClientFromPathAsync(filePath);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> IsExistsAsync(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            BlobClient blobClient = await GetBlobClientFromPathAsync(filePath);
            return await blobClient.ExistsAsync();
        }

        /// <returns>Blob blob metadata or empty object in case if not found</returns>
        public async Task<BlobDescription> TryGetBlobDescriptionAsync(string? resumeUrl)
        {
            if (await IsExistsAsync(resumeUrl))
            {
                return await GetBlobMetadataAsync(resumeUrl!);
            }
            else
            {
                return new BlobDescription();
            }
        }

        public async Task<IDictionary<string, string>> GetBlobProperties(string path)
        {
            return (await TryGetPropertiesFromRequestAsync(path)).Metadata;
        }

        public async Task SetMetadataAsync(string filePath, IDictionary<string, string> metadata)
        {
            var blobClient = await GetBlobClientFromPathAsync(filePath);
            blobClient.SetMetadata(metadata);
        }

        private string GetFileName(IFormFile formFile)
        {
            return $"{Guid.NewGuid()}{PropertiesSeparator}{formFile.FileName}";
        }

        private async Task<BlobDescription> GetBlobMetadataAsync(string filePath)
        {
            return new BlobDescription()
            {
                Name = await GetBlobName(filePath),
                Size = await GetBlobSize(filePath),
            };
        }

        private async Task<string> GetBlobName(string filePath)
        {
            var blobProperties = await GetBlobProperties(filePath);

            string fileName = string.Empty;
            if (blobProperties.ContainsKey(NameProperty))
            {
                fileName = blobProperties[NameProperty];
            }

            return fileName.ToString();
        }

        private async Task<string> GetBlobSize(string path)
        {
            var blobProperties = await TryGetPropertiesFromRequestAsync(path);
            long fileSizeInBytes = blobProperties.ContentLength;
            return FormatSizeString(fileSizeInBytes);
        }

        private Task<BlobClient> GetBlobClientFromPathAsync(string path)
        {
            var fileName = path.Split('/').Last();
            return GetBlobClientAsync(fileName);
        }

        private async Task<BlobClient> GetBlobClientAsync(string fileName)
        {
            BlobContainerClient containerClient = await GetContainerClientOrCreateIfNotExistsAsync();
            return containerClient.GetBlobClient(fileName);
        }

        private async Task<BlobContainerClient> GetContainerClientOrCreateIfNotExistsAsync()
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.Blob);
            }
            return containerClient;
        }

        private async Task<BlobProperties> TryGetPropertiesFromRequestAsync(string path)
        {
            try
            {
                BlobClient blobClient = await GetBlobClientFromPathAsync(path);
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
