using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels.Authentification;

namespace JobBoardPlatform.PL.Controllers.Register
{
    public class CompanyRegisterController : BaseRegisterController<CompanyCredentials, CompanyProfile>
    {
        public CompanyRegisterController(IRepository<CompanyCredentials> credentialsRepository,
            IRepository<CompanyProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override string GetRole()
        {
            return UserRoles.COMPANY;
        }

        protected override CompanyCredentials GetCredentials(UserRegisterViewModel userRegister)
        {
            var credentials = new CompanyCredentials()
            {
                Email = userRegister.Email,
                HashPassword = userRegister.Password,
                Profile = new CompanyProfile()
            };

            return credentials;
        }
    }
}
