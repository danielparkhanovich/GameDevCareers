using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.Authentification;

namespace JobBoardPlatform.PL.Controllers.Login
{
    public class CompanyLoginController : BaseLoginController<CompanyIdentity, CompanyProfile>
    {
        public CompanyLoginController(IRepository<CompanyIdentity> credentialsRepository,
            IRepository<CompanyProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override CompanyIdentity GetCredentials(UserLoginViewModel userLogin)
        {
            var credentials = new CompanyIdentity()
            {
                Email = userLogin.Email,
                HashPassword = userLogin.Password
            };

            return credentials;
        }
    }
}
