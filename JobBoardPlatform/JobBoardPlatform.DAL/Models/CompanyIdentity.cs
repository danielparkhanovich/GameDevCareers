using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Table("CompanyIdentities")]
    public class CompanyIdentity : IUserIdentityEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;

        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public CompanyProfile Profile { get; set; }
    }
}
