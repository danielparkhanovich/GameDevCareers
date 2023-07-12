using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Common;

namespace JobBoardPlatform.PL.ViewModels.Models.Profile.Company
{
    public class CompanyProfileViewModel : IProfileViewModel, ICompanyProfileData
    {
        public IProfileImage ProfileImage { get; set; } = new ProfileImageViewModel();
        public string CompanyName { get; set; }
        public string OfficeCountry { get; set; }
        public string OfficeCity { get; set; }
        public string? OfficeStreet { get; set; } = string.Empty;
        public string? CompanyWebsiteUrl { get; set; } = string.Empty;
    }
}
