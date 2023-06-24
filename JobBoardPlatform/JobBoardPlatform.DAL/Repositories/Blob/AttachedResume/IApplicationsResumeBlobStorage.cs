
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob.AttachedResume
{
    public interface IApplicationsResumeBlobStorage
    {
        /// <returns>url path to the resume file</returns>
        Task<string> AssignResumeToOfferAsync(int offerId, IFormFile newFile);
        Task UnassignFromOfferOnOfferClosedAsync(int offerId, string filePath);
    }
}
