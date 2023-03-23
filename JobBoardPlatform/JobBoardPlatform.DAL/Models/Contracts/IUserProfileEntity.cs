namespace JobBoardPlatform.DAL.Models.Contracts
{
    public interface IUserProfileEntity : IEntity
    {
        public string ProfileImageUrl { get; set; }
    }
}
