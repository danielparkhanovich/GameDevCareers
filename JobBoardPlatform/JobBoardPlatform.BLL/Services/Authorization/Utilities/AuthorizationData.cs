namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public class AuthorizationData
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string DisplayImageUrl { get; set; } = string.Empty;
    }
}
