using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class CompanyCredentials : ICredentialEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
    }
}
