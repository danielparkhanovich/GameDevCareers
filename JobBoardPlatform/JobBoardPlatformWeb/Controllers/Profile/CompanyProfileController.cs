using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.PL.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobBoardPlatform.DAL.Options;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Repositories.Blob;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    public class CompanyProfileController : BaseProfileController<CompanyProfile, CompanyProfileViewModel>
    {
        public CompanyProfileController(IOptions<AzureOptions> azureOptions, IRepository<CompanyProfile> profileRepository)
        {
            this.userProfileImagesStorage = new UserProfileImagesStorage(azureOptions);
            this.profileRepository = profileRepository;
            this.userViewToModel = new CompanyViewModelToProfileMapper();
        }

        [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
        public override async Task<IActionResult> Profile()
        {
            return await base.Profile();
        }

        [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
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

            return new CompanyProfileViewModel()
            {
                CompanyName = profile.CompanyName,
                City = profile.City,
                Country = profile.Country,
                ProfileImageUrl = profile.ProfileImageUrl
            };
        }

        protected override async Task UpdateProfile(CompanyProfile profile, CompanyProfileViewModel userViewModel)
        {
            if (userViewModel.ProfileImage != null)
            {
                userViewModel.ProfileImageUrl = await userProfileImagesStorage.UpdateAsync(profile.ProfileImageUrl, userViewModel.ProfileImage);
            }

            userViewToModel.Map(userViewModel, profile);

            await profileRepository.Update(profile);
        }
    }
}
