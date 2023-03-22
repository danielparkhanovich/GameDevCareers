using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public class AuthorizationData : IDisplayData
    {
        public int Id { get; set; }
        public string NameIdentifier { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;
        public string DisplayImageUrl { get; set; } = string.Empty;
    }
}
