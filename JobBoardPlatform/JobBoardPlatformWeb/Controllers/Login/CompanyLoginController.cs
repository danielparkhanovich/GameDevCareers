using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.Authentification;

namespace JobBoardPlatform.PL.Controllers.Login
{
    public class CompanyLoginController : BaseLoginController<CompanyCredentials, CompanyProfile>
    {
        public CompanyLoginController(IRepository<CompanyCredentials> credentialsRepository,
            IRepository<CompanyProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override string GetRole()
        {
            return UserRoles.COMPANY;
        }

        protected override CompanyCredentials GetCredentials(UserLoginViewModel userLogin)
        {
            var credentials = new CompanyCredentials()
            {
                Email = userLogin.Email,
                HashPassword = userLogin.Password
            };

            return credentials;
        }
    }
}
