using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using Microsoft.AspNetCore.Authorization;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.PL.ViewModels.Profile.Company;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyProfileController : BaseProfileController<CompanyProfile, CompanyProfileViewModel>
    {
        private readonly IViewModelFactory<CompanyProfile, CompanyProfileViewModel> viewModelFactory;


        public CompanyProfileController(IOptions<AzureOptions> azureOptions, 
            IRepository<CompanyProfile> profileRepository)
        {
            this.userProfileImagesStorage = new UserProfileImagesStorage(azureOptions);
            this.profileRepository = profileRepository;
            this.viewModelFactory = new CompanyProfileViewModelFactory();
        }

        protected override async Task<CompanyProfileViewModel> GetProfileViewModel()
        {
            int id = UserSessionUtils.GetProfileId(User);

            var profile = await profileRepository.Get(id);

            var profileViewModel = viewModelFactory.CreateViewModel(profile);

            return profileViewModel;
        }

        protected override async Task UpdateProfile(CompanyProfileViewModel viewModel)
        {
            int id = UserSessionUtils.GetProfileId(User);

            var updateProfileCommand = new UpdateCompanyProfileCommand(id,
                viewModel,
                profileRepository,
                HttpContext,
                userProfileImagesStorage);

            await updateProfileCommand.Execute();
        }
    }
}
