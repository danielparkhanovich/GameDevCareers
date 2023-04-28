using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Models.Contracts
{
    public interface ICompanyProfileData
    {
        IFormFile? ProfileImage { get; set; }
        string? CompanyName { get; set; }
        string? Country { get; set; }
        string? City { get; set; }
        string? ProfileImageUrl { get; set; }
        string? CompanyWebsiteUrl { get; set; }
    }
}
