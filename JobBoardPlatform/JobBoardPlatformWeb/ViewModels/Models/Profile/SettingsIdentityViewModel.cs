using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.PL.ViewModels.Models.Profile
{
    public class SettingsIdentityViewModel : ILoginSettingsData
    {
        public string? Login { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
