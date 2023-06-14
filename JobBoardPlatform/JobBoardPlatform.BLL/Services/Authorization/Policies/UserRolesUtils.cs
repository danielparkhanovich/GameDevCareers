using JobBoardPlatform.DAL.Models.Company;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public static class UserRolesUtils
    {
        private const string ReservedAdminIdentifier = "admin@gmail.com";


        public static bool IsUserAdmin(string userIdentifier)
        {
            if (ReservedAdminIdentifier == userIdentifier)
            {
                return true;
            }

            return false;
        }

        public static bool IsUserAdmin(ClaimsPrincipal user)
        {
            return UserSessionUtils.GetRole(user) == UserRoles.Admin;
        }

        public static bool IsUserEmployee(ClaimsPrincipal user)
        {
            return UserSessionUtils.GetRole(user) == UserRoles.Employee;
        }

        public static bool IsUserCompany(ClaimsPrincipal user)
        {
            return UserSessionUtils.GetRole(user) == UserRoles.Company;
        }

        public static bool IsUserOwner(ClaimsPrincipal user, JobOffer offer)
        {
            bool isUserLoggedIn = user.Identity.IsAuthenticated;
            if (!isUserLoggedIn)
            {
                return false;
            }

            string userRole = UserSessionUtils.GetRole(user);
            if (userRole != UserRoles.Company)
            {
                return false;
            }

            int userId = UserSessionUtils.GetProfileId(user);
            bool isOwner = (offer.CompanyProfileId == userId);

            return isOwner;
        }
    }
}
