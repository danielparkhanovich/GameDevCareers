using Microsoft.AspNetCore.Authorization;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Session;
using Microsoft.AspNetCore.Components;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    [Route("company")]
    public class CompanyProfileController : BaseProfileController<CompanyProfile, CompanyProfileViewModel>
    {
        private readonly IRepository<CompanyProfile> profileRepository;
        private readonly IUserProfileImagesStorage imageStorage;
        private readonly IViewModelFactory<CompanyProfile, CompanyProfileViewModel> viewModelFactory;
        private readonly IUserSessionService<CompanyIdentity, CompanyProfile> userSession;


        public CompanyProfileController(
            IRepository<CompanyProfile> profileRepository,
            IUserProfileImagesStorage imageStorage,
            IUserSessionService<CompanyIdentity, CompanyProfile> userSession)
        {
            this.profileRepository = profileRepository;
            this.imageStorage = imageStorage;
            this.userSession = userSession;
            this.viewModelFactory = new CompanyProfileViewModelFactory();
        }

        protected override async Task<CompanyProfileViewModel> GetProfileViewModel()
        {
            int id = UserSessionUtils.GetProfileId(User);
            var profile = await profileRepository.Get(id);

            var profileViewModel = viewModelFactory.Create(profile);

            return profileViewModel;
        }

        protected override async Task UpdateProfile(CompanyProfileViewModel viewModel)
        {
            int id = UserSessionUtils.GetProfileId(User);

            var updateProfileCommand = new UpdateCompanyProfileCommand(id,
                viewModel,
                profileRepository,
                imageStorage);
            await updateProfileCommand.Execute();

            await userSession.UpdateSessionStateAsync(HttpContext);
        }
    }
}
