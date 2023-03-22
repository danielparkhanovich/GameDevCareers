using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models
{
    [Index(nameof(CompanyName), IsUnique = true)]
    [Table("CompanyProfiles")]
    public class CompanyProfile : IEntity, IDisplayData
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;

        [NotMapped]
        public string DisplayName { get => CompanyName; }

        [NotMapped]
        public string DisplayImageUrl { get => PhotoUrl; }
    }
}
