using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.DAL.Repositories.Blob
{
    public interface IUserProfileImagesStorage
    {
        Task<string> ChangeImageAsync(string? path, IFormFile newFile);
        Task DeleteImageIfExistsAsync(string? path);
        Task<bool> IsImageExistsAsync(string? path);
    }
}
