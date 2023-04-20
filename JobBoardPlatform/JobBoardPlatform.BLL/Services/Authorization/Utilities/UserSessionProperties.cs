using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public static class UserSessionProperties
    {
        public const string NameIdentifier = "NameIdentifier";
        public const string ProfileIdentifier = "ProfileIdentifier";
        public const string Name = "Name";
        public const string Role = "Role";
        public const string DisplayName = "DisplayName";
        public const string DisplayImageUrl = "DisplayImageUrl";


        public static int GetIdentityId(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(NameIdentifier)!.Value);
        }

        public static int GetProfileId(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ProfileIdentifier)!.Value);
        }
    }
}
