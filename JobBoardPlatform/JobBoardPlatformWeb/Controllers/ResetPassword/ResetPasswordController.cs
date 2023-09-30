using FluentValidation;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Password;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.Filters;
using JobBoardPlatform.PL.Interactors.Notifications;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.ResetPassword
{
    [TypeFilter(typeof(RedirectLoggedInUsersFilter))]
    [Route("reset")]
    public class ResetPasswordController : Controller
    {
        private readonly IResetPasswordService<EmployeeIdentity, EmployeeProfile> employeeReset;
        private readonly IResetPasswordService<CompanyIdentity, CompanyProfile> companyReset;
        private readonly UserManager<EmployeeIdentity> employeeManager;
        private readonly UserManager<CompanyIdentity> companyManager;
        private readonly IValidator<UserPasswordViewModel> validator;


        public ResetPasswordController(
            IResetPasswordService<EmployeeIdentity, EmployeeProfile> employeeReset,
            IResetPasswordService<CompanyIdentity, CompanyProfile> companyReset,
            UserManager<EmployeeIdentity> employeeManager,
            UserManager<CompanyIdentity> companyManager,
            IValidator<UserPasswordViewModel> validator)
        {
            this.employeeReset = employeeReset;
            this.companyReset = companyReset;
            this.employeeManager = employeeManager;
            this.companyManager = companyManager;
            this.validator = validator;
        }

        public IActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(string email)
        {
            if (ModelState.IsValid)
            {
                await TryProcessResetPassword(email);
            }
            return View();
        }

        [Route("confirm/{tokenId}")]
        public IActionResult ChangePassword(string tokenId) 
        {
            var viewModel = new UserPasswordViewModel();
            return View(viewModel);
        }

        [Route("confirm/{tokenId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string tokenId, UserPasswordViewModel userPassword)
        {
            var result = await validator.ValidateAsync(userPassword);
            if (result.IsValid)
            {
                return await TryChangePassword(tokenId, userPassword);
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            return View(userPassword);
        }

        private async Task TryProcessResetPassword(string email)
        {
            if (await employeeManager.GetUserByEmailAsync(email) != null)
            {
                await ProcessResetPasswordEmployee(email);
            }
            else if (await companyManager.GetUserByEmailAsync(email) != null) 
            { 
                await ProcessResetPasswordCompany(email);
            }
            else
            {
                NotificationsManager.Instance.SetErrorNotification(
                    NotificationsManager.ResetPasswordSection,
                    $"Wrong email.",
                    TempData);
            }
        }

        private async Task ProcessResetPasswordEmployee(string email)
        {
            await employeeReset.TrySendResetPasswordTokenAsync(email);
            SetActionDoneNotification(email);
        }

        private async Task ProcessResetPasswordCompany(string email)
        {
            await companyReset.TrySendResetPasswordTokenAsync(email);
            SetActionDoneNotification(email);
        }

        private void SetActionDoneNotification(string email)
        {
            NotificationsManager.Instance.SetActionDoneNotification(
                NotificationsManager.ResetPasswordSection,
                $"An email with instructions to reset your password has been sent to your {email} email address.",
                TempData);
        }

        private async Task<IActionResult> TryChangePassword(string tokenId, UserPasswordViewModel userPassword)
        {
            string newPassword = userPassword.Password;
            try
            {
                await employeeReset.TryChangePasswordByTokenAsync(tokenId, newPassword, HttpContext);
                return RedirectToAction("Index", "Home");
            }
            catch (AuthenticationException e)
            {
            }

            try
            {
                await companyReset.TryChangePasswordByTokenAsync(tokenId, newPassword, HttpContext);
                return RedirectToAction("Index", "Home");
            }
            catch (AuthenticationException e)
            {
            }

            NotificationsManager.Instance.SetErrorNotification(
                NotificationsManager.ResetPasswordSection,
                $"Wrong email or expired token.",
                TempData);

            return View(userPassword);
        }
    }
}
