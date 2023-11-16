using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.DTOs
{
    public class AttachedResume
    {
        public IFormFile? File { get; set; }
        public string? ResumeUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
    }
}
