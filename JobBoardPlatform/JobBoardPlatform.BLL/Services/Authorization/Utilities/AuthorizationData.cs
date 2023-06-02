using JobBoardPlatform.BLL.Common.ProfileAdapter;
using JobBoardPlatform.DAL.Models.Contracts;
using System.Security.Principal;

namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public class AuthorizationData
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string DisplayImageUrl { get; set; } = string.Empty;


        public AuthorizationData(int identityId, string email, IUserProfileEntity profile)
        {
            this.Id = identityId;
            this.ProfileId = profile.Id;

            var profileAdapter = UserProfileAdapterFactory.CreateProfileAdapter(profile);
            SetDisplayData(profileAdapter);
            SetRole(profileAdapter, email);
        }

        private void SetDisplayData(IUserProfileAdapter profileAdapter)
        {
            this.DisplayName = profileAdapter.DisplayName;
            this.DisplayImageUrl = profileAdapter.DisplayProfileImageUrl;
        }

        private void SetRole(IUserProfileAdapter profileAdapter, string email)
        {
            string role = profileAdapter.UserRole;
            if (UserRolesUtils.IsUserAdmin(email))
            {
                this.Role = UserRoles.Admin;
            }
            else
            {
                this.Role = role;
            }
        }
    }
}
