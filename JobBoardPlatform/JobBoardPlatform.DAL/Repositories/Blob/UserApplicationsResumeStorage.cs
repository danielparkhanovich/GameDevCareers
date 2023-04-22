using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace JobBoardPlatform.DAL.Repositories.Blob
{
    public class UserApplicationsResumeStorage : CoreBlobStorage
    {
        private const string ContainerName = "userapplicationsresumecontainer";
        private const string OfferIdProperty = "offerId";

        private readonly BlobHttpHeaders blobHttpHeaders;
        private readonly int offerId;


        public UserApplicationsResumeStorage(IOptions<AzureOptions> azureOptions, int offerId) : base(azureOptions)
        {
            blobHttpHeaders = new BlobHttpHeaders()
            {
                ContentType = "application/pdf"
            };

            this.offerId = offerId;
        }

        protected override Dictionary<string, string> GetMetadata(IFormFile file)
        {
            var metadata = base.GetMetadata(file);

            metadata[OfferIdProperty] = offerId.ToString();

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
