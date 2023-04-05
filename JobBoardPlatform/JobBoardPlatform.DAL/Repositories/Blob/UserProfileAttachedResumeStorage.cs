using Azure.Storage.Blobs.Models;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;

namespace JobBoardPlatform.DAL.Repositories.Blob
{
    public class UserProfileAttachedResumeStorage : CoreBlobStorage
    {
        private const string ContainerName = "userprofileattachedresumecontainer";

        private readonly BlobHttpHeaders blobHttpHeaders;


        public UserProfileAttachedResumeStorage(IOptions<AzureOptions> azureOptions) : base(azureOptions)
        {
            blobHttpHeaders = new BlobHttpHeaders()
            {
                ContentType = "application/pdf"
            };
        }

        protected override string GetContainerName()
        {
            return ContainerName;
        }

        protected override BlobHttpHeaders GetBlobHttpHeaders()
        {
            return blobHttpHeaders;
        }
    }
}
