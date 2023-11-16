using JobBoardPlatform.BLL.DTOs;

namespace JobBoardPlatform.PL.ViewModels.Models.Profile
{
    public class SettingsIdentityViewModel : LoginSettingsData
    {
        public string? Login { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
