using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile.Employee
{
    public class EmployeeProfileViewModel : IProfileViewModel, IAttachedResume
    {
        public string? ProfileImageUrl { get => Display.ProfileImageUrl; }
        public IFormFile? ProfileImage { get => Update.ProfileImage; set => Update.ProfileImage = value; }

        public IFormFile? File { get => Update.AttachedResume.File; set => Update.AttachedResume.File = value; }
        public string? ResumeUrl { get => Update.AttachedResume.ResumeUrl; set => Update.AttachedResume.ResumeUrl = value; }
        public string? FileName { get => Update.AttachedResume.FileName; set => Update.AttachedResume.FileName = value; }
        public string? FileSize { get => Update.AttachedResume.FileSize; set => Update.AttachedResume.FileSize = value; }

        public EmployeeProfileDisplayViewModel Display { get; set; }
        public EmployeeProfileUpdateViewModel Update { get; set; }


        public EmployeeProfileViewModel()
        {
            Display = new EmployeeProfileDisplayViewModel();
            Update = new EmployeeProfileUpdateViewModel()
            {
                AttachedResume = new EmployeeAttachedResumeViewModel()
            };
        }
    }
}
