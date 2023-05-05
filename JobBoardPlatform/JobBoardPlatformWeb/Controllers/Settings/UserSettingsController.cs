using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Security
{
    [Authorize]
    public class UserSettingsController : Controller
    {
        private readonly IRepository<EmployeeIdentity> employeeIdentityRepository;
        private readonly IRepository<CompanyIdentity> companyIdentityRepository;


        public UserSettingsController(IRepository<EmployeeIdentity> employeeIdentityRepository,
            IRepository<CompanyIdentity> companyIdentityRepository)
        {
            this.employeeIdentityRepository = employeeIdentityRepository;
            this.companyIdentityRepository = companyIdentityRepository;
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
                    await UpdateIdentity(employeeIdentityRepository, viewModel);
                }
                else if (userRole == UserRoles.Company)
                {
                    await UpdateIdentity(companyIdentityRepository, viewModel);
                }
                else
                {
                    throw new Exception("Unsupported user role!");
                }
            }

            return RedirectToAction("Settings");
        }

        private async Task UpdateIdentity<TIdentity>(IRepository<TIdentity> identityRepository,
            SettingsIdentityViewModel viewModel) 
            where TIdentity : class, IUserIdentityEntity
        {
            var session = new ModifyIdentityService<TIdentity>(identityRepository);

            int identityId = UserSessionUtils.GetIdentityId(User);
            var identity = await identityRepository.Get(identityId);

            AuthentificationResult result = null;

            if (!string.IsNullOrEmpty(viewModel.LoginIdentifier) && 
                 identity!.Email != viewModel.LoginIdentifier)
            {
                result = await session.TryChangeLoginIdentifier(identity!, viewModel.LoginIdentifier);
                if (result.IsError)
                {
                    ModelState.AddModelError("AlreadyExistsError", result.Error);
                }
            }
            else if (!string.IsNullOrEmpty(viewModel.OldPassword) && 
                     !string.IsNullOrEmpty(viewModel.NewPassword))
            {
                result = await session.ChangePassword(identity!, viewModel.OldPassword, viewModel.NewPassword);
                if (result.IsError)
                {
                    ModelState.AddModelError("AlreadyExistsError", result.Error);
                }
            }
        }

        private async Task<string> GetLoginIdentifier()
        {
            int identityId = UserSessionUtils.GetIdentityId(User);
            var userRole = UserSessionUtils.GetRole(User);

            string loginIdentifier = string.Empty;

            if (userRole == UserRoles.Employee)
            {
                var employeeIdentity = await employeeIdentityRepository.Get(identityId);
                loginIdentifier = employeeIdentity!.Email;
            }
            else if (userRole == UserRoles.Company)
            {
                var companyIdentity = await companyIdentityRepository.Get(identityId);
                loginIdentifier = companyIdentity!.Email;
            }

            return loginIdentifier;
        }
    }
}
