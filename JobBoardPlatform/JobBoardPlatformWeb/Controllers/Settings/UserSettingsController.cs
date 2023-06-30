using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Security
{
    [Authorize]
    public class UserSettingsController : Controller
    {
        private readonly IModifyIdentityService<EmployeeIdentity> employeeModifyService;
        private readonly IModifyIdentityService<CompanyIdentity> companyModifyService;


        public UserSettingsController(
            IModifyIdentityService<EmployeeIdentity> employeeModifyService,
            IModifyIdentityService<CompanyIdentity> companyModifyService)
        {
            this.employeeModifyService = employeeModifyService;
            this.companyModifyService = companyModifyService;
        }

        [Route("settings")]
        public async Task<IActionResult> Settings()
        {
            string loginIdentifier = await GetLoginIdentifier();
            var viewModel = new SettingsIdentityViewModel()
            {
                LoginIdentifier = loginIdentifier,
            };
            return View(viewModel);
        }

        [Route("settings")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsIdentityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userRole = UserSessionUtils.GetRole(User);

                if (userRole == UserRoles.Employee)
                {
                    await UpdateIdentity(viewModel, employeeModifyService);
                }
                else if (userRole == UserRoles.Company)
                {
                    await UpdateIdentity(viewModel, companyModifyService);
                }
                else
                {
                    throw new Exception("Unsupported user role!");
                }
            }

            return RedirectToAction("Settings");
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int userId)
        {
            //var deleteCommand = GetDeleteCommand(userId);
            //await deleteCommand.Execute();
            return Ok();
        }

        private async Task UpdateIdentity<TIdentity>(
            SettingsIdentityViewModel viewModel, IModifyIdentityService<TIdentity> modifyService) 
            where TIdentity : class, IUserIdentityEntity
        {
            int identityId = UserSessionUtils.GetIdentityId(User);

            if (IsChangeLogin("TOOD: Replace", viewModel.LoginIdentifier))
            {
                await TryChangeLoginAsync(modifyService, identityId, "TOOD: Replace", viewModel.LoginIdentifier!);
            }
            else if (IsChangePassword(viewModel.OldPassword, viewModel.NewPassword))
            {
                await TryChangePasswordAsync(modifyService, identityId, viewModel);
            }
        }

        private async Task<string> GetLoginIdentifier()
        {
            int identityId = UserSessionUtils.GetIdentityId(User);
            var userRole = UserSessionUtils.GetRole(User);

            string loginIdentifier = string.Empty;

            if (userRole == UserRoles.Employee)
            {
                // var employeeIdentity = await employeeIdentityRepository.Get(identityId);
                // loginIdentifier = employeeIdentity!.Email;
            }
            else if (userRole == UserRoles.Company)
            {
                // var companyIdentity = await companyIdentityRepository.Get(identityId);
                // loginIdentifier = companyIdentity!.Email;
            }
            return "TODO: replace";
            return loginIdentifier;
        }

        private bool IsChangeLogin(string oldLogin, string? newLogin)
        {
            return !string.IsNullOrEmpty(newLogin) && oldLogin != newLogin;
        }

        private async Task TryChangeLoginAsync<TIdentity>(
            IModifyIdentityService<TIdentity> modify, int userId, string currentPassword, string newLogin)
            where TIdentity : class, IUserIdentityEntity
        {
            try
            {
                await modify.TryChangeLoginIdentifierAsync(userId, newLogin, currentPassword);
            }
            catch (AuthenticationException e)
            {
                ModelState.AddModelError("AlreadyExistsError", e.Message);
            }
        }

        private bool IsChangePassword(string? oldPassword, string? newPassword)
        {
            return !string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword);
        }

        private async Task TryChangePasswordAsync<TIdentity>(
            IModifyIdentityService<TIdentity> modify, int userId, SettingsIdentityViewModel viewModel)
            where TIdentity : class, IUserIdentityEntity
        {
            try
            {
                await modify.TryChangePasswordAsync(userId, viewModel.OldPassword!, viewModel.NewPassword!);
            }
            catch (AuthenticationException e)
            {
                ModelState.AddModelError("AlreadyExistsError", e.Message);
            }
        }
    }
}
