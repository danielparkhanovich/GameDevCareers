using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile.Employee
{
    public class EmployeeProfileViewModel : IProfileViewModel
    {
        public string? ProfileImageUrl { get => Display.ProfileImageUrl; }
        public IFormFile? ProfileImage { get => Update.ProfileImage; set => Update.ProfileImage = value; }

        public EmployeeProfileDisplayViewModel Display { get; set; }
        public EmployeeProfileUpdateViewModel Update { get; set; }


        public EmployeeProfileViewModel()
        {
            Display = new EmployeeProfileDisplayViewModel();
            Update = new EmployeeProfileUpdateViewModel();
        }
    }
}
