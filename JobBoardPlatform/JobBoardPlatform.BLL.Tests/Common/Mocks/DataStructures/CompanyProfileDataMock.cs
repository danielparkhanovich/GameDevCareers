using JobBoardPlatform.BLL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.DataStructures
{
    public class CompanyProfileDataMock : ICompanyProfileData
    {
        public IFormFile? ProfileImage { get; set; }
        public string? CompanyName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? CompanyWebsiteUrl { get; set; }
    }
}
