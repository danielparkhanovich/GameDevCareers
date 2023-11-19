using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.Controllers.Presenters;
using JobBoardPlatform.PL.ViewModels.Models.Profile;
using JobBoardPlatformWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Security
{
    [Authorize]
    public class UserSettingsController : Controller
    {
        private readonly IUserSettingsService settingsService;
        private readonly IAuthorizationService<EmployeeIdentity, EmployeeProfile> employeeAuthorization;
        private readonly IAuthorizationService<CompanyIdentity, CompanyProfile> companyAuthorization;


        public UserSettingsController(
            IUserSettingsService settingsService,
            IAuthorizationService<EmployeeIdentity, EmployeeProfile> employeeAuthorization,
            IAuthorizationService<CompanyIdentity, CompanyProfile> companyAuthorization)
        {
            this.settingsService = settingsService;
            this.employeeAuthorization = employeeAuthorization;
            this.companyAuthorization = companyAuthorization;
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

            return RedirectToAction(nameof(Settings));
        }

        [Route("settings/delete")]
        public async Task<IActionResult> Delete()
        {
            await DeleteAccount();

            return RedirectToAction(
                nameof(HomeController.Index), 
                ControllerUtils.GetControllerName(typeof(HomeController)));
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

        private async Task DeleteAccount()
        {
            int identityId = UserSessionUtils.GetIdentityId(User);
            var userRole = UserSessionUtils.GetRole(User);
            await settingsService.DeleteAccountAsync(userRole, identityId);

            if (UserRolesUtils.IsUserEmployee(User))
            {
                await employeeAuthorization.SignOutHttpContextAsync(HttpContext);
            }
            else if (UserRolesUtils.IsUserCompany(User))
            {
                await companyAuthorization.SignOutHttpContextAsync(HttpContext);
            }
        }
    }
}
