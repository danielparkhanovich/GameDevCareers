using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public static class UserSessionUtils
    {
        public static int GetIdentityId(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(UserSessionProperties.NameIdentifier)!.Value);
        }

        public static int GetProfileId(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(UserSessionProperties.ProfileIdentifier)!.Value);
        }

        public static string GetRole(ClaimsPrincipal user)
        {
            return user.FindFirst(UserSessionProperties.Role)!.Value;
        }
    }
}
