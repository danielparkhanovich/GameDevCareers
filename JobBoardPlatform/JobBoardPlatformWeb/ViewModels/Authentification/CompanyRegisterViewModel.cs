using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Authentification
{
    public class CompanyRegisterViewModel : UserRegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string CompanyName { get; set; } = string.Empty;
    }
}
