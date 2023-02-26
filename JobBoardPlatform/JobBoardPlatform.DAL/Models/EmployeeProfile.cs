using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.DAL.Models
{
    public class EmployeeProfile : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
    }
}
