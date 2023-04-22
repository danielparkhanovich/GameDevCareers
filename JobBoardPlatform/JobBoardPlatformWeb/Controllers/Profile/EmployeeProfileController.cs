using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.PL.ViewModels.Profile.Employee;
using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Utilities.Mappers.Profile;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.EmployeeOnlyPolicy)]
    public class EmployeeProfileController : BaseProfileController<EmployeeProfile, EmployeeProfileViewModel>
    {
        protected IBlobStorage userProfileResumeStorage;


        public EmployeeProfileController(IOptions<AzureOptions> azureOptions, IRepository<EmployeeProfile> profileRepository)
        {
            this.userProfileImagesStorage = new UserProfileImagesStorage(azureOptions);
            this.profileRepository = profileRepository;
            this.userViewToModel = new EmployeeViewModelToProfileMapper();
            
            this.userProfileResumeStorage = new UserProfileAttachedResumeStorage(azureOptions);
        }

        public override async Task<IActionResult> Profile()
        {
            return await base.Profile();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Profile(EmployeeProfileViewModel userViewModel)
        {
            return await base.Profile(userViewModel);
        }

        public async Task<IActionResult> DeleteResume()
        {
            int id = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
            var profile = await profileRepository.Get(id);

            await userProfileResumeStorage.DeleteAsync(profile.ResumeUrl!);
            profile.ResumeUrl = null;

            await profileRepository.Update(profile);

            var userSession = new UserSessionService<EmployeeProfile>(HttpContext);
            await userSession.UpdateSessionStateAsync(profile);

            return RedirectToAction("Profile");
        }

        // TODO: remove display and update here, put everyting inside of EmployeeProfileViewModel
        protected override async Task<EmployeeProfileViewModel> UpdateProfileDisplay()
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

            var display = new EmployeeProfileDisplayViewModel()
            {
                Name = profile.Name,
                Surname = profile.Surname,
                City = profile.City,
                Country = profile.Country,
                Description = profile.Description,
                ProfileImageUrl = profile.ProfileImageUrl,
                AttachedResumeUrl = profile.ResumeUrl,
                AttachedResumeFileName = blobName,
                AttachedResumeFileSize = blobSize,
                YearsOfExperience = profile.YearsOfExperience,
                LinkedInUrl = profile.LinkedInUrl
            };

            var employeeProfileViewModel = new EmployeeProfileViewModel()
            {
                Display = display
            };
            employeeProfileViewModel.Update.Description = profile.Description;

            var attachedResume = new EmployeeAttachedResumeViewModel();
            attachedResume.ResumeUrl = profile.ResumeUrl;
            attachedResume.FileName = blobName;
            attachedResume.FileSize = blobSize;
            employeeProfileViewModel.Update.AttachedResume = attachedResume;

            return employeeProfileViewModel;
        }

        protected override async Task UpdateProfile(EmployeeProfile profile, EmployeeProfileViewModel userViewModel)
        {
            var updateViewModel = userViewModel.Update;

            // TODO: validate data here for stream size
            // and extension... and add a model error

            if (updateViewModel.ProfileImage != null)
            {
                var imageUrl = await userProfileImagesStorage.UpdateAsync(profile.ProfileImageUrl, updateViewModel.ProfileImage);
                profile.ProfileImageUrl = imageUrl;
            }
            if (updateViewModel.AttachedResume != null && updateViewModel.AttachedResume.File != null)
            {
                var resumeUrl = await userProfileResumeStorage.UpdateAsync(profile.ResumeUrl, updateViewModel.AttachedResume.File);
                updateViewModel.AttachedResume.ResumeUrl = resumeUrl;
            }

            userViewToModel.Map(userViewModel, profile);

            await profileRepository.Update(profile);
        }
    }
}
