namespace JobBoardPlatform.BLL.Services.Common.Contracts
{
    internal interface IUserProfileAdapter
    {
        string DisplayName { get; }
        string DisplayProfileImageUrl { get; }
        string UserRole { get; }
    }
}
