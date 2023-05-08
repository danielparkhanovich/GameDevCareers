using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    public class CompanyRegisterController : BaseRegisterController<CompanyIdentity, CompanyProfile, CompanyRegisterViewModel>
    {
        public CompanyRegisterController(IRepository<CompanyIdentity> credentialsRepository,
            IRepository<CompanyProfile> profileRepository)
        {
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        public IActionResult RegisterPromotion()
        {
            return View();
        }

        protected override CompanyIdentity GetIdentity(CompanyRegisterViewModel companyRegister)
        {
            var companyProfile = new CompanyProfile();
            companyProfile.CompanyName = companyRegister.CompanyName;

            var credentials = new CompanyIdentity()
            {
                Email = companyRegister.Email,
                HashPassword = companyRegister.Password
            };

            credentials.Profile = companyProfile;

            return credentials;
        }
    }
}
