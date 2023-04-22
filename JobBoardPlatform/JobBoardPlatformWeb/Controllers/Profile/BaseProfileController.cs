using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Profile.Contracts;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    public abstract class BaseProfileController<TProfile, TViewModel> : Controller
        where TProfile : class, IUserProfileEntity
        where TViewModel : class, IProfileViewModel
    {
        protected IRepository<TProfile> profileRepository;
        protected IMapper<TViewModel, TProfile> userViewToModel;
        protected IBlobStorage userProfileImagesStorage;


        public async virtual Task<IActionResult> Profile()
        {
            var viewModel = await UpdateProfileDisplay();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Profile(TViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
                var profile = await profileRepository.Get(id);

                await UpdateProfile(profile, userViewModel);

                var userSession = new UserSessionService<TProfile>(HttpContext);
                await userSession.UpdateSessionStateAsync(profile);

                return RedirectToAction("Profile");
            }

            userViewModel = await UpdateProfileDisplay();

            ModelState.Clear();

            return View(userViewModel);
        }

        protected abstract Task UpdateProfile(TProfile profile, TViewModel userViewModel);
        protected abstract Task<TViewModel> UpdateProfileDisplay();
    }
}
