namespace JobBoardPlatform.BLL.Services.Common.ProfileAdapter.Contracts
{
    internal interface IUserProfileAdapter
    {
        string DisplayName { get; }
        string DisplayProfileImageUrl { get; }
        string UserRole { get; }
    }
}
