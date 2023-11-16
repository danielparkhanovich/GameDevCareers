using JobBoardPlatform.BLL.DTOs;

namespace JobBoardPlatform.PL.ViewModels.Models.Authentification
{
    public class UserRegisterViewModel : UserLoginData
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RepeatPassword { get; set; } = string.Empty;
        public bool IsAcceptedTermsOfService { get; set; } = false;
    }
}
