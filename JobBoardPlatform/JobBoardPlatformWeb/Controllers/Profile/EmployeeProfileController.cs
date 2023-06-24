using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Employee;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Session;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.EmployeeOnlyPolicy)]
    public class EmployeeProfileController : BaseProfileController<EmployeeProfile, EmployeeProfileViewModel>
    {
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IUserProfileImagesStorage imageStorage;
        private readonly IProfileResumeBlobStorage resumeStorage;
        private readonly IUserSessionService<EmployeeIdentity, EmployeeProfile> userSession;


        public EmployeeProfileController(
            IRepository<EmployeeProfile> profileRepository,
            IUserProfileImagesStorage imageStorage,
            IProfileResumeBlobStorage resumeStorage,
            IUserSessionService<EmployeeIdentity, EmployeeProfile> userSession)
        {
            this.imageStorage = imageStorage;
            this.resumeStorage = resumeStorage;
            this.profileRepository = profileRepository;            
            this.userSession = userSession;
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

            var deleteResumeCommand = new DeleteEmployeeResumeCommand(
                id, profileRepository, resumeStorage);

            await deleteResumeCommand.Execute();
            await userSession.UpdateSessionStateAsync(HttpContext);

            return RedirectToAction("Profile");
        }

        protected override async Task<EmployeeProfileViewModel> GetProfileViewModel()
        {
            int id = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

            var profile = await profileRepository.Get(id);
            var metadata = await resumeStorage.GetMetadataAsync(profile.ResumeUrl);
            string resumeName = metadata.Name;
            string resumeSize = metadata.Size;

            var viewModelFactory = new EmployeeProfileViewModelFactory(resumeName, resumeSize);
            var viewModel = viewModelFactory.Create(profile);

            return viewModel;
        }

        protected override async Task UpdateProfile(EmployeeProfileViewModel viewModel)
        {
            int id = UserSessionUtils.GetProfileId(User);

            var updateProfileCommand = new UpdateEmployeeProfileCommand(id,
                viewModel,
                profileRepository,
                imageStorage,
                resumeStorage);

            await updateProfileCommand.Execute();

            await userSession.UpdateSessionStateAsync(HttpContext);
        }
    }
}
