using JobBoardPlatform.BLL.Services.MessageBus.Notifications;
using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    public abstract class BaseProfileController<TProfile, TViewModel> : Controller
        where TProfile : class, IUserProfileEntity
        where TViewModel : class, IProfileViewModel
    {
        protected IRepository<TProfile> profileRepository;
        protected IBlobStorage userProfileImagesStorage;


        public async Task<IActionResult> Profile()
        {
            var viewModel = await GetProfileViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Profile(TViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await UpdateProfile(userViewModel);

                TempData["OperationResultMessage"] = "Profile successfully updated";
                TempData["OperationResultStatus"] = OperationResultType.Success.ToString();

                return RedirectToAction("Profile");
            }

            userViewModel = await GetProfileViewModel();

            return View(userViewModel);
        }

        protected abstract Task UpdateProfile(TViewModel userViewModel);
        protected abstract Task<TViewModel> GetProfileViewModel();
    }
}
