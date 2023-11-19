using JobBoardPlatform.DAL.Models.Company;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authentification.Authorization
{
    public static class UserRolesUtils
    {
        public static bool IsUserAdmin(ClaimsPrincipal user)
        {
            return UserSessionUtils.IsLoggedIn(user) && 
                   UserSessionUtils.GetRole(user) == UserRoles.Admin;
        }

        public static bool IsUserEmployee(ClaimsPrincipal user)
        {
            return UserSessionUtils.IsLoggedIn(user) && 
                   UserSessionUtils.GetRole(user) == UserRoles.Employee;
        }

        public static bool IsUserCompany(ClaimsPrincipal user)
        {
            return UserSessionUtils.IsLoggedIn(user) && 
                   UserSessionUtils.GetRole(user) == UserRoles.Company;
        }

        public static bool IsUserOwner(ClaimsPrincipal user, JobOffer offer)
        {
            if (!UserSessionUtils.IsLoggedIn(user))
            {
                return false;
            }

            string userRole = UserSessionUtils.GetRole(user);
            if (userRole != UserRoles.Company)
            {
                return false;
            }

            int userId = UserSessionUtils.GetProfileId(user);
            bool isOwner = offer.CompanyProfileId == userId;

            return isOwner;
        }
    }
}
