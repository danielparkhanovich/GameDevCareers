using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.PL.ViewModels.Profile.Company;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Company.Company;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyProfileController : BaseProfileController<CompanyProfile, CompanyProfileViewModel>
    {
        public CompanyProfileController(IOptions<AzureOptions> azureOptions, IRepository<CompanyProfile> profileRepository)
        {
            this.userProfileImagesStorage = new UserProfileImagesStorage(azureOptions);
            this.profileRepository = profileRepository;
            this.userViewToModel = new CompanyViewModelToProfileMapper();
        }

        public override async Task<IActionResult> Profile()
        {
            return await base.Profile();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Profile(CompanyProfileViewModel userViewModel)
        {
            return await base.Profile(userViewModel);
        }

        protected override async Task<CompanyProfileViewModel> UpdateProfileDisplay()
        {
            int id = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

            var profile = await profileRepository.Get(id);

            var display = new CompanyProfileDisplayViewModel()
            {
                CompanyName = profile.CompanyName,
                City = profile.City,
                Country = profile.Country,
                ProfileImageUrl = profile.ProfileImageUrl,
                CompanyWebsiteUrl = profile.CompanyWebsiteUrl
            };

            return new CompanyProfileViewModel()
            {
                Display = display
            };
        }

        protected override async Task UpdateProfile(CompanyProfile profile, CompanyProfileViewModel userViewModel)
        {
            var updateViewModel = userViewModel.Update;

            // TODO: validate data here for stream size
            // and extension... and add a model error

            if (updateViewModel.ProfileImage != null)
            {
                var imageUrl = await userProfileImagesStorage.UpdateAsync(profile.ProfileImageUrl, updateViewModel.ProfileImage);
                profile.ProfileImageUrl = imageUrl;
            }

            userViewToModel.Map(userViewModel, profile);

            await profileRepository.Update(profile);
        }
    }
}
