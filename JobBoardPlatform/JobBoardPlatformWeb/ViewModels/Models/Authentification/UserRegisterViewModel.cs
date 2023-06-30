using JobBoardPlatform.BLL.Models.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Authentification
{
    public class UserRegisterViewModel : IUserLoginData
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RepeatPassword { get; set; } = string.Empty;
        public bool IsAcceptedTermsOfService { get; set; } = false;
    }
}
