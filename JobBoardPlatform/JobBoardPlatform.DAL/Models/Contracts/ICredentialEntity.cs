namespace JobBoardPlatform.DAL.Models.Contracts
{
    public interface ICredentialEntity : IEntity
    {
        string Email { get; set; }
        string HashPassword { get; set; }
        int ProfileId { get; set; }
    }
}
