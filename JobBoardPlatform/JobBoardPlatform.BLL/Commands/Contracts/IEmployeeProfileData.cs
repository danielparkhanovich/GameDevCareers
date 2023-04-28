using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Models.Contracts
{
    public interface IEmployeeProfileData
    {
        public IFormFile? ProfileImage { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }

        public IFormFile? File { get; set; }
        public string? ResumeUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
    }
}
