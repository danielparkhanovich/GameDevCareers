using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile.Company
{
    public class CompanyProfileUpdateViewModel : ICompanyProfileSharedFields
    {
        public IFormFile? ProfileImage { get; set; }
        public string? CompanyName { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; } = string.Empty;
        public string? CompanyWebsiteUrl { get; set; } = string.Empty;
    }
}
