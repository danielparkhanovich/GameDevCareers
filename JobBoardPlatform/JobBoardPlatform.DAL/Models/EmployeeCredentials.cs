using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.DAL.Models
{
    public class EmployeeCredentials : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
    }
}
