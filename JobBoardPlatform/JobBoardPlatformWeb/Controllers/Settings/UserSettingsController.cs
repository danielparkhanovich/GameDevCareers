using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Security
{
    [Authorize]
    public class UserSettingsController : Controller
    {
        private readonly IUserSettingsService settingsService;


        public UserSettingsController(IUserSettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        [Route("settings")]
        public async Task<IActionResult> Settings()
        {
            string login = await GetLogin();
            var viewModel = new SettingsIdentityViewModel()
            {
                Login = login,
            };
            return View(viewModel);
        }

        [HttpPost("settings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsIdentityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await TryUpdateLoginData(viewModel);
            }

            return RedirectToAction("Settings");
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete()
        {
            await DeleteAccount();
            return Ok();
        }

        private Task<string> GetLogin()
        {
            int identityId = UserSessionUtils.GetIdentityId(User);
            var userRole = UserSessionUtils.GetRole(User);
            return settingsService.GetLoginAsync(userRole, identityId);
        }

        private Task TryUpdateLoginData(SettingsIdentityViewModel viewModel)
        {
            int identityId = UserSessionUtils.GetIdentityId(User);
            var userRole = UserSessionUtils.GetRole(User);
            return settingsService.TryUpdateLoginDataAsync(userRole, identityId, viewModel);
        }

        private Task DeleteAccount()
        {
            int identityId = UserSessionUtils.GetIdentityId(User);
            var userRole = UserSessionUtils.GetRole(User);
            return settingsService.DeleteAccountAsync(userRole, identityId);
        }
    }
}
