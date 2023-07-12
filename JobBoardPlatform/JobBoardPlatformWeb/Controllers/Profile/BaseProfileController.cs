using JobBoardPlatform.BLL.Services.MessageBus.Notifications;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    public abstract class BaseProfileController<TProfile, TViewModel> : Controller
        where TProfile : class, IUserProfileEntity
        where TViewModel : class, IProfileViewModel
    {
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
