using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Repositories.Blob.Exceptions;
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using JobBoardPlatform.DAL.Repositories.Blob.Settings;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace JobBoardPlatform.DAL.Repositories.Blob.AttachedResume
{
    public sealed class UserProfileAttachedResumeStorage : IProfileResumeBlobStorage
    {
        private const string RelatedOffersProperty = "relatedOffersIds";
        private const string IsAttachedToProfileProperty = "isAttachedToProfile";

        private readonly string containerName;

        private readonly CoreBlobStorage blobStorage;
        private readonly BlobHttpHeaders httpHeaders;


        public UserProfileAttachedResumeStorage(CoreBlobStorage blobStorage, IBlobStorageSettings storageSettings)
        {
            this.blobStorage = blobStorage;
            this.containerName = storageSettings.GetContainerName(this.GetType());

            this.httpHeaders = new BlobHttpHeaders()
            {
                ContentType = "application/pdf"
            };
        }

        public async Task<string> ChangeResumeAsync(string? path, IFormFile newFile)
        {
            await DetachResumeFromProfileAndTryDeleteAsync(path);

            var exportData = GetExportData(newFile);
            AddResumeProperties(exportData.Metadata);

            return await blobStorage.AddAsync(exportData, containerName);
        }

        public Task<BlobDescription> GetMetadataAsync(string? resumeUrl)
        {
            return blobStorage.TryGetBlobDescriptionAsync(resumeUrl, containerName);
        }

        public async Task AssignResumeToOfferAsync(int offerId, string filePath)
        {
            var metadata = await blobStorage.GetBlobProperties(filePath, containerName);
            AddOfferIdToRelatedOffersProperty(offerId, metadata);
            await blobStorage.SetMetadataAsync(filePath, metadata, containerName);
        }

        public async Task UnassignFromOfferOnOfferClosedAsync(int offerId, string filePath)
        {
            if (!await blobStorage.IsExistsAsync(filePath, containerName))
            {
                return;
            }

            var metadata = await blobStorage.GetBlobProperties(filePath, containerName);
            RemoveOfferIdToRelatedOffersProperty(offerId, metadata);
            await UpdateOrDeleteFile(filePath, metadata);
        }

        public async Task DetachResumeFromProfileAndTryDeleteAsync(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            try
            {
                var metadata = await blobStorage.GetBlobProperties(filePath, containerName);
                SetAttachedToProfileProperty(false, metadata);
                await UpdateOrDeleteFile(filePath, metadata);
            }
            catch (BlobStorageException e)
            {
                await blobStorage.DeleteIfExistsAsync(filePath, containerName);
            }
        }

        public Task<bool> IsExistsAsync(string? filePath)
        {
            return blobStorage.IsExistsAsync(filePath, containerName);
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

        private void AddResumeProperties(IDictionary<string, string> metadata)
        {
            AddPropertyToMetadata(RelatedOffersProperty, new List<int>(), metadata);
            AddPropertyToMetadata(IsAttachedToProfileProperty, true, metadata);
        }

        private void AddPropertyToMetadata<T>(
            string propertyName, T propertyValue, IDictionary<string, string> metadata)
        {
            string serializedValue = JsonSerializer.Serialize(propertyValue);
            metadata.Add(propertyName, serializedValue);
        }

        private void AddOfferIdToRelatedOffersProperty(int offerId, IDictionary<string, string> metadata)
        {
            var appliedOffersIds = GetPropertyValue<List<int>>(RelatedOffersProperty, metadata);
            appliedOffersIds.Add(offerId);
            metadata[RelatedOffersProperty] = JsonSerializer.Serialize(appliedOffersIds);
        }

        private void RemoveOfferIdToRelatedOffersProperty(int offerId, IDictionary<string, string> metadata)
        {
            var appliedOffersIds = GetPropertyValue<List<int>>(RelatedOffersProperty, metadata);
            appliedOffersIds.Remove(offerId);
            metadata[RelatedOffersProperty] = JsonSerializer.Serialize(appliedOffersIds);
        }

        private void SetAttachedToProfileProperty(bool value, IDictionary<string, string> metadata)
        {
            metadata[IsAttachedToProfileProperty] = JsonSerializer.Serialize(value);
        }

        private async Task UpdateOrDeleteFile(string filePath, IDictionary<string, string> metadata)
        {
            if (IsDelete(metadata))
            {
                await blobStorage.DeleteIfExistsAsync(filePath, containerName);
            }
            else
            {
                await blobStorage.SetMetadataAsync(filePath, metadata, containerName);
            }
        }

        private bool IsDelete(IDictionary<string, string> metadata)
        {
            var appliedOffersIds = GetPropertyValue<List<int>>(RelatedOffersProperty, metadata);
            var isAttachedToProfile = GetPropertyValue<bool>(IsAttachedToProfileProperty, metadata);
            return appliedOffersIds.Count == 0 && !isAttachedToProfile;
        }

        private T GetPropertyValue<T>(string property, IDictionary<string, string> metadata)
        {
            if (!metadata.ContainsKey(property))
            {
                throw new BlobStorageException("Property not found");
            }
            return JsonSerializer.Deserialize<T>(metadata[property])!;
        }
    }
}
