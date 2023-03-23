using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels.Authentification;

namespace JobBoardPlatform.PL.Controllers.Register
{
    public class EmployeeRegisterController : BaseRegisterController<EmployeeIdentity, EmployeeProfile>
    {
        public EmployeeRegisterController(IRepository<EmployeeIdentity> credentialsRepository,
            IRepository<EmployeeProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override string GetRole()
        {
            return UserRoles.Employee;
        }

        protected override EmployeeIdentity GetCredentials(UserRegisterViewModel userRegister)
        {
            var credentials = new EmployeeIdentity()
            {
                Email = userRegister.Email,
                HashPassword = userRegister.Password,
                Profile = new EmployeeProfile()
            };

            return credentials;
        }
    }
}
