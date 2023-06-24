using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Repositories.Blob.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;

namespace JobBoardPlatform.DAL.Repositories.Blob.AttachedResume
{
    public class UserProfileAttachedResumeStorage : IProfileResumeBlobStorage
    {
        public const string ContainerName = "userprofileattachedresumecontainer";
        protected const string RelatedOffersProperty = "appliedOffersIds";

        private readonly CoreBlobStorage blobStorage;
        private readonly BlobHttpHeaders httpHeaders;


        public UserProfileAttachedResumeStorage(CoreBlobStorage blobStorage)
        {
            this.blobStorage = blobStorage;
            this.blobStorage.SetContainerName(ContainerName);

            this.httpHeaders = new BlobHttpHeaders()
            {
                ContentType = "application/pdf"
            };
        }

        public async Task<string> ChangeResumeAsync(string? path, IFormFile newFile)
        {
            await DeleteIfNotAssignedToOffersAsync(path);

            var exportData = GetExportData(newFile);
            AddRelatedOffersIdsProperty(exportData.Metadata);

            return await blobStorage.AddAsync(exportData);
        }

        public Task<BlobDescription> GetMetadataAsync(string? resumeUrl)
        {
            return blobStorage.TryGetBlobDescriptionAsync(resumeUrl);
        }

        public async Task AssignResumeToOfferAsync(int offerId, string filePath)
        {
            var metadata = await blobStorage.GetBlobProperties(filePath);
            var appliedOffersIds = GetAppliedOffersIds(metadata);
            appliedOffersIds.Add(offerId);
            metadata[RelatedOffersProperty] = JsonSerializer.Serialize(appliedOffersIds);
            await blobStorage.SetMetadataAsync(filePath, metadata);
        }

        public async Task DeleteIfNotAssignedToOffersAsync(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            try
            {
                await TryDeleteIfNotAssignedToOffersAsync(filePath);
            }
            catch (BlobStorageException e)
            {
                await blobStorage.DeleteIfExistsAsync(filePath);
            }
        }

        public Task<bool> IsExistsAsync(string? filePath)
        {
            return blobStorage.IsExistsAsync(filePath);
        }

        private BlobExportData GetExportData(IFormFile file)
        {
            return new BlobExportData()
            {
                File = file,
                BlobHttpHeaders = httpHeaders,
                Metadata = new Dictionary<string, string>()
            };
        }

        private void AddRelatedOffersIdsProperty(IDictionary<string, string> metadata)
        {
            AddPropertyToExportMetadata(RelatedOffersProperty, new List<int>(), metadata);
        }

        private void AddPropertyToExportMetadata<T>(
            string propertyName, T propertyValue, IDictionary<string, string> metadata)
        {
            string serializedValue = JsonSerializer.Serialize(propertyValue);
            metadata.Add(propertyName, serializedValue);
        }

        private async Task TryDeleteIfNotAssignedToOffersAsync(string filePath)
        {
            var metadata = await blobStorage.GetBlobProperties(filePath);
            if (!metadata.ContainsKey(RelatedOffersProperty))
            {
                throw new BlobStorageException("Property not found");
            }

            var appliedOffersIds = GetAppliedOffersIds(metadata);
            if (appliedOffersIds.Count == 0)
            {
                await blobStorage.DeleteIfExistsAsync(filePath);
                return;
            }
        }

        private List<int> GetAppliedOffersIds(IDictionary<string, string> metadata)
        {
            return JsonSerializer.Deserialize<List<int>>(metadata[RelatedOffersProperty])!;
        }
    }
}
