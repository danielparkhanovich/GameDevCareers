using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Repositories.Blob.Exceptions;
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using JobBoardPlatform.DAL.Repositories.Blob.Settings;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace JobBoardPlatform.DAL.Repositories.Blob.AttachedResume
{
    public sealed class UserApplicationsResumeStorage : IApplicationsResumeBlobStorage
    {
        private const string RelatedOffersProperty = "relatedOffersIds";

        private readonly string containerName = "userapplicationsresumecontainer";
        private readonly CoreBlobStorage blobStorage;
        private readonly BlobHttpHeaders httpHeaders;


        public UserApplicationsResumeStorage(CoreBlobStorage blobStorage, IBlobStorageSettings storageSettings)
        {
            this.blobStorage = blobStorage;
            this.containerName = storageSettings.GetContainerName(this.GetType());

            this.httpHeaders = new BlobHttpHeaders()
            {
                ContentType = "application/pdf"
            };
        }

        public async Task<string> AssignResumeToOfferAsync(int offerId, IFormFile newFile)
        {
            var exportData = GetExportData(newFile);
            AddRelatedOffersIdsProperty(offerId, exportData.Metadata);

            return await blobStorage.AddAsync(exportData, containerName);
        }

        public async Task UnassignFromOfferOnOfferClosedAsync(int offerId, string filePath)
        {
            try 
            {
                await UnassignFromOfferAndTryDeleteAsync(offerId, filePath);
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

        private async Task UnassignFromOfferAndTryDeleteAsync(int offerId, string filePath)
        {
            (var appliedOffersIds, var metadata) = await GetUpdatedOffersListFromMetadataAsync(offerId, filePath);
            if (appliedOffersIds.Count == 0)
            {
                await blobStorage.DeleteIfExistsAsync(filePath, containerName);
                return;
            }
            await blobStorage.SetMetadataAsync(filePath, metadata, containerName);
        }

        private async Task<(List<int>, IDictionary<string, string>)> GetUpdatedOffersListFromMetadataAsync(
            int offerId, string filePath)
        {
            (var appliedOffersIds, var metadata) = await GetRelatedOffersFromMetadataAsync(filePath);
            appliedOffersIds.Remove(offerId);

            metadata[RelatedOffersProperty] = JsonSerializer.Serialize(appliedOffersIds);

            return (appliedOffersIds, metadata);
        }

        /// <returns>(updated offers ids after removed offerId, updated metadata)</returns>
        private async Task<(List<int>, IDictionary<string, string>)> GetRelatedOffersFromMetadataAsync(string filePath)
        {
            var metadata = await blobStorage.GetBlobProperties(filePath, containerName);

            if (!metadata.ContainsKey(RelatedOffersProperty))
            {
                throw new BlobStorageException("Property not found");
            }

            var appliedOffersIds = JsonSerializer.Deserialize<List<int>>(metadata[RelatedOffersProperty])!;
            return (appliedOffersIds, metadata);
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

        private void AddRelatedOffersIdsProperty(int offerId, IDictionary<string, string> metadata)
        {
            AddPropertyToExportMetadata(RelatedOffersProperty, new List<int>() { offerId }, metadata);
        }

        private void AddPropertyToExportMetadata<T>(
            string propertyName, T propertyValue, IDictionary<string, string> metadata)
        {
            string serializedValue = JsonSerializer.Serialize(propertyValue);
            metadata.Add(propertyName, serializedValue);
        }
    }
}
