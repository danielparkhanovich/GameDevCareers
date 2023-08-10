using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Utils
{
    public static class StaticFilesUtils
    {
        private const string PathToCompanyDefaultAvatar = "/Resources/defaultCompany.png";
        private const string PathToEmployeeDefaultAvatar = "/Resources/defaultUserProfileImage.png";


        public static string GetDefaultAvatarUriIfEmpty(string? imageUrl, ClaimsPrincipal user)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return imageUrl;
            }   

            if (UserRolesUtils.IsUserCompany(user))
            {
                return PathToCompanyDefaultAvatar;
            }
            else
            {
                return PathToEmployeeDefaultAvatar;
            }
        }

        public static string GetCompanyDefaultAvatarUriIfEmpty(string? imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return imageUrl;
            }

            return PathToCompanyDefaultAvatar;
        }

        public static string GetEmployeeDefaultAvatarUriIfEmpty(string? imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return imageUrl;
            }

            return PathToEmployeeDefaultAvatar;
        }
    }
}
