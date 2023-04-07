using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Authentification
{
    public class UserRegisterViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
