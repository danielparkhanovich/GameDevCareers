using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Common;

namespace JobBoardPlatform.PL.ViewModels.Models.Profile.Employee
{
    public class EmployeeProfileViewModel : EmployeeProfileData, IProfileViewModel
    {
        public EmployeeAttachedResumeViewModel? AttachedResume { get; set; } = new EmployeeAttachedResumeViewModel();
    }
}
