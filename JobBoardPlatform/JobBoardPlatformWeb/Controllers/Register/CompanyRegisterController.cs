using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Authentification;

namespace JobBoardPlatform.PL.Controllers.Register
{
    public class CompanyRegisterController : BaseRegisterController<CompanyIdentity, CompanyProfile>
    {
        public CompanyRegisterController(IRepository<CompanyIdentity> credentialsRepository,
            IRepository<CompanyProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override CompanyIdentity GetIdentity(UserRegisterViewModel userRegister)
        {
            var credentials = new CompanyIdentity()
            {
                Email = userRegister.Email,
                HashPassword = userRegister.Password,
                Profile = new CompanyProfile()
            };

            return credentials;
        }
    }
}
