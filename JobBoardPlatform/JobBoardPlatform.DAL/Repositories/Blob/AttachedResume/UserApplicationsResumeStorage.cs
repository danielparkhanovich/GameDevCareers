using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace JobBoardPlatform.DAL.Repositories.Blob.AttachedResume
{
    public class UserApplicationsResumeStorage : CoreBlobStorage
    {
        public const string ContainerName = "userapplicationsresumecontainer";
        private const string OfferIdProperty = "offerId";

        private readonly BlobHttpHeaders blobHttpHeaders;
        private readonly int offerId;
        private Dictionary<string, string> additionalMetadata;


        public UserApplicationsResumeStorage(IOptions<AzureOptions> azureOptions) : base(azureOptions)
        {
            blobHttpHeaders = new BlobHttpHeaders()
            {
                ContentType = "application/pdf"
            };

            additionalMetadata = new Dictionary<string, string>();
        }

        public void SetOfferIdProperty(string value)
        {
            additionalMetadata.Add(OfferIdProperty, value);
        }

        protected override Dictionary<string, string> GetMetadata(IFormFile file)
        {
            var metadata = base.GetMetadata(file);

            foreach (var kvp in additionalMetadata)
            {
                metadata.Add(kvp.Key, kvp.Value);
            }

            return metadata;
        }

        protected override string GetContainerName()
        {
            return ContainerName;
        }

        protected override BlobHttpHeaders GetBlobHttpHeaders()
        {
            return blobHttpHeaders;
        }

        protected override string GetFileName(IFormFile file)
        {
            return $"{Guid.NewGuid()}{PropertiesSeparator}{offerId}{PropertiesSeparator}{file.FileName}";
        }
    }
}
