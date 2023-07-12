using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Common;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Models.Profile.Employee
{
    public class EmployeeProfileViewModel : IProfileViewModel, IEmployeeProfileData, IAttachedResume
    {
        public IProfileImage ProfileImage { get; set; } = new ProfileImageViewModel();
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public EmployeeAttachedResumeViewModel? AttachedResume { get; set; }
        public string? YearsOfExperience { get; set; } = string.Empty;
        [Url]
        public string? LinkedInUrl { get; set; } = string.Empty;

        public IFormFile? File { get; set; }
        public string? ResumeUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
    }
}
