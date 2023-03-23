namespace JobBoardPlatform.DAL.Models.Contracts
{
    public interface IUserIdentityEntity : IEntity
    {
        string Email { get; set; }
        string HashPassword { get; set; }
        int ProfileId { get; set; }
    }
}
