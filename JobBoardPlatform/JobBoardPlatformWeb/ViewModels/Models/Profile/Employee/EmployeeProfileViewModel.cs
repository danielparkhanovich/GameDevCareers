using JobBoardPlatform.BLL.Commands.Contracts;
using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Contracts;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Models.Profile.Employee
{
    public class EmployeeProfileViewModel : IProfileViewModel, IAttachedResume, IEmployeeProfileData
    {
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; } = string.Empty;
        public EmployeeAttachedResumeViewModel? AttachedResume { get; set; }
        public string? YearsOfExperience { get; set; } = string.Empty;
        [Url]
        public string? LinkedInUrl { get; set; } = string.Empty;

        public IFormFile? File { get; set; }
        public string? ResumeUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
