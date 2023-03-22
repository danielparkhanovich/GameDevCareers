using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    public class CompanyProfileController : BaseProfileController<CompanyProfile, CompanyProfileViewModel>
    {
        public CompanyProfileController(IRepository<CompanyProfile> profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        [Authorize(Policy = AuthorizationPolicies.COMPANY_ONLY_POLICY)]
        public override async Task<IActionResult> Profile()
        {
            var viewModel = await GetUserViewModel();

            return View(viewModel);
        }

        [Authorize(Policy = AuthorizationPolicies.COMPANY_ONLY_POLICY)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Profile(CompanyProfileViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(User.FindFirstValue("Id"));
                var profile = await profileRepository.Get(id);

                var mapper = new CompanyViewModelToProfileMapper();
                mapper.Map(userViewModel, profile);

                await profileRepository.Update(profile);

                var sessionManager = new AuthorizationService(HttpContext);
                await sessionManager.SignOutHttpContextAsync();
                var authorization = new AuthorizationData()
                {
                    Id = id,
                    NameIdentifier = profile.CompanyName,
                    DisplayImageUrl = profile.PhotoUrl,
                    DisplayName = profile.CompanyName,
                    Role = UserRoles.COMPANY
                };
                await sessionManager.SignInHttpContextAsync(authorization);

                return RedirectToAction("Profile");
            }

            userViewModel = await GetUserViewModel();

            return View(userViewModel);
        }

        private async Task<CompanyProfileViewModel> GetUserViewModel()
        {
            int id = int.Parse(User.FindFirstValue("Id"));

            var profile = await profileRepository.Get(id);

            return new CompanyProfileViewModel()
            {
                CompanyName = profile.CompanyName,
                City = profile.City,
                Country = profile.Country,
                PhotoUrl = profile.PhotoUrl
            };
        }
    }
}
