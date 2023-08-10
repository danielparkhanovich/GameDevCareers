using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob.Temporary
{
    public interface IUserProfileImagesTemporaryStorage
    {
        Task<string> ChangeImageAsync(string? path, IFormFile newFile);
        Task DeleteImageIfExistsAsync(string? path);
    }
}
