using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.PL.ViewModels.Profile.Employee;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.EmployeeOnlyPolicy)]
    public class EmployeeProfileController : BaseProfileController<EmployeeProfile, EmployeeProfileViewModel>
    {
        protected IBlobStorage userProfileResumeStorage;


        public EmployeeProfileController(IOptions<AzureOptions> azureOptions, 
            IRepository<EmployeeProfile> profileRepository)
        {
            this.userProfileImagesStorage = new UserProfileImagesStorage(azureOptions);
            this.profileRepository = profileRepository;            
            this.userProfileResumeStorage = new UserProfileAttachedResumeStorage(azureOptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Profile(EmployeeProfileViewModel userViewModel)
        {
            return await base.Profile(userViewModel);
        }

        public async Task<IActionResult> DeleteResume()
        {
            int id = UserSessionUtils.GetProfileId(User);

            var updateProfileCommand = new DeleteEmployeeResumeCommand(id,
                profileRepository,
                userProfileResumeStorage,
                HttpContext);

            await updateProfileCommand.Execute();

            return RedirectToAction("Profile");
        }

        protected override async Task<EmployeeProfileViewModel> GetProfileViewModel()
        {
            int id = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

            var profile = await profileRepository.Get(id);

            string blobName = string.Empty;
            string blobSize = string.Empty;
            if (!string.IsNullOrEmpty(profile.ResumeUrl))
            {
                blobName = await userProfileResumeStorage.GetBlobName(profile.ResumeUrl);
                blobSize = await userProfileResumeStorage.GetBlobSize(profile.ResumeUrl);
            }

            var viewModelFactory = new EmployeeProfileViewModelFactory(blobName, blobSize);
            var viewModel = viewModelFactory.CreateViewModel(profile);

            return viewModel;
        }

        protected override async Task UpdateProfile(EmployeeProfileViewModel viewModel)
        {
            int id = UserSessionUtils.GetProfileId(User);

            var updateProfileCommand = new UpdateEmployeeProfileCommand(id,
                viewModel,
                profileRepository,
                HttpContext,
                userProfileImagesStorage,
                userProfileResumeStorage);

            await updateProfileCommand.Execute();
        }
    }
}
