namespace JobBoardPlatform.DAL.Models.Contracts
{
    public interface IProfileEntity : IEntity
    {
        string DisplayName { get; }
    }
}
