using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels.Authentification;

namespace JobBoardPlatform.PL.Controllers.Register
{
    public class EmployeeRegisterController : BaseRegisterController<EmployeeCredentials, EmployeeProfile>
    {
        public EmployeeRegisterController(IRepository<EmployeeCredentials> credentialsRepository,
            IRepository<EmployeeProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override string GetRole()
        {
            return UserRoles.EMPLOYEE;
        }

        protected override EmployeeCredentials GetCredentials(UserRegisterViewModel userRegister)
        {
            var credentials = new EmployeeCredentials()
            {
                Email = userRegister.Email,
                HashPassword = userRegister.Password,
                Profile = new EmployeeProfile()
            };

            return credentials;
        }
    }
}
