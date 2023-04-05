using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile.Employee
{
    public class EmployeeProfileUpdateViewModel
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
    }
}
