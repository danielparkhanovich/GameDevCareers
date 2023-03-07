using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Contracts
{
    public interface ICredentialEntity : IEntity
    {
        string Email { get; set; }
        string HashPassword { get; set; }
    }
}
