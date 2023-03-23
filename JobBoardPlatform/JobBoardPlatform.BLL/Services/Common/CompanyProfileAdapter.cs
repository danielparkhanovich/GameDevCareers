using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Common.Contracts;
using JobBoardPlatform.DAL.Models;

namespace JobBoardPlatform.BLL.Services.Common
{
    internal class CompanyProfileAdapter : IUserProfileAdapter
    {
        private readonly CompanyProfile profile;


        public string DisplayName => profile.CompanyName;

        public string DisplayProfileImageUrl => profile.ProfileImageUrl;

        public string UserRole => UserRoles.Company;


        public CompanyProfileAdapter(CompanyProfile profile) 
        {
            this.profile = profile;
        }
    }
}
