using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Utils
{
    public static class StaticFilesUtils
    {
        private const string PathToCompanyDefaultAvatar = "/Resources/defaultCompany.png";
        private const string PathToEmployeeDefaultAvatar = "/Resources/defaultUserProfileImage.png";
        private const string PathToTechnologyWidgetDefaultIcon = "/Resources/MainTechnology/defaultWidget.svg";

        public const string PathToTechnologyAllWidgetIcon = "/Resources/MainTechnology/all.svg";
        public const string PathToTechnologyProgrammingWidgetIcon = "/Resources/MainTechnology/programming.svg";
        public const string PathToTechnologyAudioWidgetIcon = "/Resources/MainTechnology/audio.svg";
        public const string PathToTechnologyGraphicsWidgetIcon = "/Resources/MainTechnology/graphics3d.svg";
        public const string PathToTechnologyLevelDesignWidgetIcon = "/Resources/MainTechnology/levelDesign.svg";
        public const string PathToTechnologyManagementWidgetIcon = "/Resources/MainTechnology/management.svg";
        public const string PathToTechnologyTestingWidgetIcon = "/Resources/MainTechnology/testing.svg";
        public const string PathToTechnologyOtherWidgetIcon = "/Resources/MainTechnology/other.svg";


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
            return GetDefaultUriIfEmpty(imageUrl, PathToCompanyDefaultAvatar);
        }

        public static string GetEmployeeDefaultAvatarUriIfEmpty(string? imageUrl)
        {
            return GetDefaultUriIfEmpty(imageUrl, PathToEmployeeDefaultAvatar);
        }

        public static string GetDefaultTechnologyWidgetUriIfEmpty(string? imageUrl) 
        {
            return GetDefaultUriIfEmpty(imageUrl, PathToTechnologyWidgetDefaultIcon);
        }

        private static string GetDefaultUriIfEmpty(string? imageUrl, string defaultUri)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return imageUrl;
            }

            return defaultUri;
        }
    }
}
