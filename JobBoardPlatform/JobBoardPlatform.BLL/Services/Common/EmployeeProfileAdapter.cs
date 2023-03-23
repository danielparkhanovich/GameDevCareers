using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Common.Contracts;
using JobBoardPlatform.DAL.Models;

namespace JobBoardPlatform.BLL.Services.Common
{
    internal class EmployeeProfileAdapter : IUserProfileAdapter
    {
        private readonly EmployeeProfile profile;


        public string DisplayName => profile.Name;

        public string DisplayProfileImageUrl => profile.ProfileImageUrl;

        public string UserRole => UserRoles.Employee;


        public EmployeeProfileAdapter(EmployeeProfile profile) 
        {
            this.profile = profile;
        }
    }
}
