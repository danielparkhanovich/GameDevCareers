using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Common.ProfileAdapter
{
    // TODO: rename into CompanyClaimsAdapter
    internal class CompanyProfileAdapter : IUserProfileAdapter
    {
        private readonly CompanyProfile profile;


        public string DisplayName => profile.CompanyName;

        public string DisplayProfileImageUrl
        {
            get
            {
                if (profile.ProfileImageUrl == null)
                {
                    return string.Empty;
                }
                else
                {
                    return profile.ProfileImageUrl;
                }
            }
        }

        public string UserRole => UserRoles.Company;


        public CompanyProfileAdapter(CompanyProfile profile)
        {
            this.profile = profile;
        }
    }
}
