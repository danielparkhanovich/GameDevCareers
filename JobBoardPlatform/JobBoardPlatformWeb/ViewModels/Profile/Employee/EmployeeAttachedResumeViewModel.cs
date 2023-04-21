using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile.Employee
{
    public class EmployeeAttachedResumeViewModel : IAttachedResume
    {
        public IFormFile? File { get; set; }
        public string? ResumeUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }
    }
}
