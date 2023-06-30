using FluentValidation;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.Interactors.Notifications;
using JobBoardPlatform.PL.Interactors.Registration;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    [Route("register-employee")]
    public class EmployeeRegisterController : BaseRegisterController<UserRegisterViewModel>
    {
        private readonly IRegistrationInteractor<UserRegisterViewModel> registrationInteractor;
        private readonly ILoginService<CompanyIdentity, CompanyProfile> loginService;
        private readonly IValidator<UserRegisterViewModel> validator;


        public EmployeeRegisterController(
            IRegistrationInteractor<UserRegisterViewModel> registrationInteractor,
            ILoginService<CompanyIdentity, CompanyProfile> loginService,
            IValidator<UserRegisterViewModel> validator)
        {
            this.registrationInteractor = registrationInteractor;
            this.loginService = loginService;
            this.validator = validator;
        }

        public override async Task<IActionResult> Register(UserRegisterViewModel userRegister)
        {
            var result = await validator.ValidateAsync(userRegister);
            if (result.IsValid)
            {
                var redirect = await TryProcessRegistration(userRegister);
                if (redirect != RedirectData.NoRedirect)
                {
                    return RedirectToAction(redirect.ActionName, redirect.Data);
                }
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            if (ModelState.ErrorCount == 0) 
            {
                userRegister = new UserRegisterViewModel();
            }
            return View(userRegister);
        }

        public override async Task<IActionResult> TryConfirmRegistration(string tokenId)
        {
            await registrationInteractor.FinishRegistration(tokenId, HttpContext);
            return RedirectToAction("Index", "Home");
        }

        public override async Task<IActionResult> TryConfirmRegistration(string email, string passwordHash)
        {
            await loginService.TryLoginAsync(email, passwordHash, HttpContext);
            return RedirectToAction("Index", "Home");
        }

        private async Task<RedirectData> TryProcessRegistration(UserRegisterViewModel userRegister)
        {
            try
            {
                var redirect = await registrationInteractor.ProcessRegistrationAndRedirect(userRegister);
                NotificationsManager.Instance.SetActionDoneNotification(
                    NotificationsManager.RegisterSection, 
                    $"Check your email inbox {userRegister.Email} for a confirmation link to complete your registration.", 
                    TempData);
                return redirect;
            }
            catch (AuthenticationException e)
            {
                ModelState.AddModelError("Email", e.Message);
                return RedirectData.NoRedirect;
            }
        }
    }
}
