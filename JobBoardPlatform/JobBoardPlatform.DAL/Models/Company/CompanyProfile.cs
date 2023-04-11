using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Index(nameof(CompanyName), IsUnique = true)]
    [Table("CompanyProfiles")]
    public class CompanyProfile : IUserProfileEntity
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? CompanyWebsiteUrl { get; set; }
    }
}
