using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Boundaries
{
    public interface ICompanyProfileData
    {
        public IProfileImage ProfileImage { get; set; }
        string? CompanyName { get; set; }
        string? OfficeCountry { get; set; }
        string? OfficeCity { get; set; }
        string? CompanyWebsiteUrl { get; set; }
    }
}
