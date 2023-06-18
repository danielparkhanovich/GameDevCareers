using Microsoft.AspNetCore.Authorization;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Session;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyProfileController : BaseProfileController<CompanyProfile, CompanyProfileViewModel>
    {
        private readonly IViewModelFactory<CompanyProfile, CompanyProfileViewModel> viewModelFactory;
        private readonly IUserSessionService<CompanyIdentity, CompanyProfile> userSession;


        public CompanyProfileController(
            IOptions<AzureOptions> azureOptions, 
            IRepository<CompanyProfile> profileRepository,
            IUserSessionService<CompanyIdentity, CompanyProfile> userSession)
        {
            this.userProfileImagesStorage = new UserProfileImagesStorage(azureOptions);
            this.profileRepository = profileRepository;
            this.viewModelFactory = new CompanyProfileViewModelFactory();
            this.userSession = userSession;
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
                HttpContext,
                userSession,
                userProfileImagesStorage);

            await updateProfileCommand.Execute();
        }
    }
}
