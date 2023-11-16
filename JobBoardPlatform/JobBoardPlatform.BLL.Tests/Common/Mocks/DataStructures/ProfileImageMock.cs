using JobBoardPlatform.BLL.DTOs;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.DataStructures
{
    public class ProfileImageMock : ProfileImage
    {
        public IFormFile? File { get; set; }
        public string? ImageUrl { get; set; }
    }
}
