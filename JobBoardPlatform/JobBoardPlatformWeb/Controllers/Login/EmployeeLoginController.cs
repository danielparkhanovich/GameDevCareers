using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.Authentification;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.PL.Controllers.Login
{
    public class EmployeeLoginController : BaseLoginController<EmployeeIdentity, EmployeeProfile>
    {
        public EmployeeLoginController(IRepository<EmployeeIdentity> credentialsRepository,
            IRepository<EmployeeProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override EmployeeIdentity GetIdentity(UserLoginViewModel userLogin)
        {
            var credentials = new EmployeeIdentity()
            {
                Email = userLogin.Email,
                HashPassword = userLogin.Password
            };

            return credentials;
        }
    }
}
