using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.DataStructures
{
    public class CompanyProfileDataMock : ICompanyProfileData
    {
        public IProfileImage ProfileImage { get; set; } = new ProfileImageMock();
        public string? CompanyName { get; set; }
        public string? OfficeCountry { get; set; }
        public string? OfficeCity { get; set; }
        public string? OfficeStreet { get; set; }
        public string? CompanyWebsiteUrl { get; set; }
    }
}
