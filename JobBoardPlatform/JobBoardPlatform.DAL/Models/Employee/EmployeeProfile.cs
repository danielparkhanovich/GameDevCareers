using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Employee
{
    [Table("EmployeeProfiles")]
    public class EmployeeProfile : IUserProfileEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Surname { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
    }
}
