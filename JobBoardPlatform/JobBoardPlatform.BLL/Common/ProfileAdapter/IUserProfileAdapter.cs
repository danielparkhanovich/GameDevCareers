namespace JobBoardPlatform.BLL.Common.ProfileAdapter
{
    internal interface IUserProfileAdapter
    {
        string DisplayName { get; }
        string DisplayProfileImageUrl { get; }
        string UserRole { get; }
    }
}
