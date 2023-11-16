
namespace JobBoardPlatform.BLL.DTOs
{
    public class CompanyProfileData : ProfileData
    {
        public string? CompanyName { get; set; }
        public string? OfficeCountry { get; set; }
        public string? OfficeCity { get; set; }
        public string? OfficeStreet { get; set; }
        public string? CompanyWebsiteUrl { get; set; }
    }
}
