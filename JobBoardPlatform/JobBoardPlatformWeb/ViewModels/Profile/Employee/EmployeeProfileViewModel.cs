using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile.Employee
{
    public class EmployeeProfileViewModel : IProfileViewModel
    {
        public EmployeeProfileDisplayViewModel? Display { get; set; }
        public EmployeeProfileUpdateViewModel? Update { get; set; }
    }
}
