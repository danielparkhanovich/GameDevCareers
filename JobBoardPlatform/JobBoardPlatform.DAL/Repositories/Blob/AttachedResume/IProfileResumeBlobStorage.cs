
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob.AttachedResume
{
    public interface IProfileResumeBlobStorage
    {
        Task<string> ChangeResumeAsync(string? path, IFormFile newFile);
        Task AssignResumeToOfferAsync(int offerId, string filePath);
        Task UnassignFromOfferOnOfferClosedAsync(int offerId, string filePath);
        Task DetachResumeFromProfileAndTryDeleteAsync(string? filePath);
        Task<BlobDescription> GetMetadataAsync(string? resumeUrl);
        Task<bool> IsExistsAsync(string? filePath);
    }
}
