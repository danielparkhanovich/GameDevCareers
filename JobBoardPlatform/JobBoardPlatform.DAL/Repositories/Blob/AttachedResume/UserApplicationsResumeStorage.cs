using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace JobBoardPlatform.DAL.Repositories.Blob.AttachedResume
{
    public class UserApplicationsResumeStorage : IApplicationsResumeBlobStorage
    {
        public const string ContainerName = "userapplicationsresumecontainer";
        protected const string RelatedOffersProperty = "relatedOffersIds";

        private readonly CoreBlobStorage blobStorage;
        private readonly BlobHttpHeaders httpHeaders;


        public UserApplicationsResumeStorage(CoreBlobStorage blobStorage)
        {
            this.blobStorage = blobStorage;
            this.blobStorage.SetContainerName(ContainerName);

            this.httpHeaders = new BlobHttpHeaders()
            {
                ContentType = "application/pdf"
            };
        }

        public async Task<string> AssignResumeToOfferAsync(int offerId, IFormFile newFile)
        {
            var exportData = GetExportData(newFile);
            AddRelatedOffersIdsProperty(offerId, exportData.Metadata);

            return await blobStorage.AddAsync(exportData);
        }

        public async Task UnassignFromOfferOnOfferClosedAsync(int offerId, string filePath)
        {
            (var appliedOffersIds, var metadata) = await GetUpdatedOffersListFromMetadataAsync(offerId, filePath);
            if (appliedOffersIds.Count == 0)
            {
                await blobStorage.DeleteIfExistsAsync(filePath);
                return;
            }

            await blobStorage.SetMetadataAsync(filePath, metadata);
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
            var metadata = await blobStorage.GetBlobProperties(filePath);
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
