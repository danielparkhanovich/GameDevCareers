using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.PL.ViewModels.Authentification;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Filters;
using Microsoft.AspNetCore.Mvc;

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

        protected override CompanyIdentity GetIdentity(UserLoginViewModel userLogin)
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
