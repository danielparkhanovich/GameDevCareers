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
    public class EmployeeProfileController : BaseProfileController<EmployeeProfile, EmployeeProfileViewModel>
    {
        public EmployeeProfileController(IRepository<EmployeeProfile> profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        [Authorize(Policy = AuthorizationPolicies.EmployeeOnlyPolicy)]
        public override async Task<IActionResult> Profile()
        {
            var viewModel = await GetUserViewModel();

            return View(viewModel);
        }

        [Authorize(Policy = AuthorizationPolicies.EmployeeOnlyPolicy)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Profile(EmployeeProfileViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(User.FindFirstValue("Id"));
                var profile = await profileRepository.Get(id);

                var mapper = new EmployeeViewModelToProfileMapper();
                mapper.Map(userViewModel, profile);

                await profileRepository.Update(profile);

                var sessionManager = new AuthorizationService(HttpContext);
                await sessionManager.SignOutHttpContextAsync();
                var authorization = new AuthorizationData()
                {
                    Id = id,
                    DisplayImageUrl = profile.ProfileImageUrl,
                    DisplayName = profile.Name,
                    Role = UserRoles.Employee
                };
                await sessionManager.SignInHttpContextAsync(authorization);

                return RedirectToAction("Profile");
            }

            userViewModel = await GetUserViewModel();

            return View(userViewModel);
        }

        private async Task<EmployeeProfileViewModel> GetUserViewModel()
        {
            int id = int.Parse(User.FindFirstValue("Id"));

            var profile = await profileRepository.Get(id);

            return new EmployeeProfileViewModel()
            {
                Name = profile.Name,
                Surname = profile.Surname,
                City = profile.City,
                Country = profile.Country,
                Description = profile.Description,
                PhotoUrl = profile.ProfileImageUrl,
                ResumeUrl = profile.ResumeUrl
            };
        }
    }
}
