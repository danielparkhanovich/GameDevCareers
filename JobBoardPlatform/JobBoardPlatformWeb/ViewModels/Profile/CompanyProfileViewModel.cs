using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile
{
    public class CompanyProfileViewModel : IProfileViewModel
    {
        public IFormFile? Avatar { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
