using JobBoardPlatform.BLL.Services.Session;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public static class UserSessionUtils
    {
        public static bool IsLoggedIn(ClaimsPrincipal user)
        {
            if (TryGetRole(user) == null)
            {
                return false;
            }
            return user.Identity!.IsAuthenticated;
        }

        public static bool IsHasAnyRole(ClaimsPrincipal user)
        {
            return user.FindFirst(UserSessionProperties.Role) != null;
        }

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
            return TryGetRole(user)!;
        }

        private static string? TryGetRole(ClaimsPrincipal user)
        {
            return user.FindFirst(UserSessionProperties.Role)?.Value;
        }
    }
}
