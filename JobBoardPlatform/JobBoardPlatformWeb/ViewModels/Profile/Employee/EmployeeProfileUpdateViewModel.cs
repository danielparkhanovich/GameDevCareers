using JobBoardPlatform.PL.ViewModels.Profile.Contracts;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Profile.Employee
{
    public class EmployeeProfileUpdateViewModel : IEmployeeProfileSharedFields
    {
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? AttachedResume { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; } = string.Empty;
        public string? AttachedResumeUrl { get; set; } = string.Empty;
        public string? YearsOfExperience { get; set; } = string.Empty;
        [Url]
        public string? LinkedInUrl { get; set; } = string.Empty;
    }
}
