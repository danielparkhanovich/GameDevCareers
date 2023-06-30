using JobBoardPlatform.BLL.Common.ProfileAdapter;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Authorization
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
            Id = identityId;
            ProfileId = profile.Id;

            var profileAdapter = UserProfileAdapterFactory.CreateProfileAdapter(profile);
            SetDisplayData(profileAdapter);
            SetRole(profileAdapter, email);
        }

        private void SetDisplayData(IUserProfileAdapter profileAdapter)
        {
            DisplayName = profileAdapter.DisplayName;
            DisplayImageUrl = profileAdapter.DisplayProfileImageUrl;
        }

        private void SetRole(IUserProfileAdapter profileAdapter, string email)
        {
            string role = profileAdapter.UserRole;
            if (UserRolesUtils.IsUserAdmin(email))
            {
                Role = UserRoles.Admin;
            }
            else
            {
                Role = role;
            }
        }
    }
}
