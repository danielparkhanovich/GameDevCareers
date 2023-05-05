using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;

namespace JobBoardPlatform.PL.Controllers.Register
{
    public class EmployeeRegisterController : BaseRegisterController<EmployeeIdentity, EmployeeProfile, UserRegisterViewModel>
    {
        public EmployeeRegisterController(IRepository<EmployeeIdentity> credentialsRepository,
            IRepository<EmployeeProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override EmployeeIdentity GetIdentity(UserRegisterViewModel userRegister)
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
