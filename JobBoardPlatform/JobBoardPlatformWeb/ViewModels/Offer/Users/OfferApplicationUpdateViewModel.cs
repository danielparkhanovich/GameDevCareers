using JobBoardPlatform.PL.ViewModels.Profile.Employee;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.OfferViewModels.Users
{
    public class OfferApplicationUpdateViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public EmployeeAttachedResumeViewModel AttachedResume { get; set; }

        public string AdditionalInformation { get; set; } = string.Empty;
    }
}
