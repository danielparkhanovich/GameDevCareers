using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Boundaries
{
    public interface IProfileImage
    {
        public IFormFile? File { get; set; }
        public string? ImageUrl { get; set; }
    }
}
