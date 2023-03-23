using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models
{
    [Table("EmployeeProfiles")]
    public class EmployeeProfile : IUserProfileEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
    }
}
