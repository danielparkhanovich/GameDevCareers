using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Interactors.Registration;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    [Route("register-company")]
    public class CompanyRegisterController : BaseRegisterController<CompanyRegisterViewModel>
    {
        private readonly IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor;
        private readonly ILoginService<CompanyIdentity, CompanyProfile> loginService;


        public CompanyRegisterController(
            IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor,
            ILoginService<CompanyIdentity, CompanyProfile> loginService)
        {
            this.registrationInteractor = registrationInteractor;
            this.loginService = loginService;
        }

        [Route("pricing")]
        public IActionResult RegisterPromotion()
        {
            return View();
        }

        public override async Task<IActionResult> Register(CompanyRegisterViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var redirect = await registrationInteractor.ProcessRegistrationAndRedirect(userRegister);
                return RedirectToAction(redirect.ActionName, redirect.Data);
            }

            return View(userRegister);
        }

        public override async Task<IActionResult> TryConfirmRegistration(string tokenId)
        {
            await registrationInteractor.FinishRegistration(tokenId, HttpContext);
            return View();
        }

        public override async Task<IActionResult> TryConfirmRegistration(string email, string passwordHash)
        {
            await loginService.TryLoginAsync(email, passwordHash, HttpContext);
            return View();
        }
    }
}
