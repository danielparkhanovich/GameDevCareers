using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.Authentification;

namespace JobBoardPlatform.PL.Controllers.Login
{
    public class EmployeeLoginController : BaseLoginController<EmployeeCredentials, EmployeeProfile>
    {
        public EmployeeLoginController(IRepository<EmployeeCredentials> credentialsRepository,
            IRepository<EmployeeProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override string GetRole()
        {
            return UserRoles.EMPLOYEE;
        }

        protected override EmployeeCredentials GetCredentials(UserLoginViewModel userLogin)
        {
            var credentials = new EmployeeCredentials()
            {
                Email = userLogin.Email,
                HashPassword = userLogin.Password
            };

            return credentials;
        }
    }
}
