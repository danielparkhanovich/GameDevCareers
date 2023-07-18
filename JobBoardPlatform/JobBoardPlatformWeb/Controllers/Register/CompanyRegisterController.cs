using FluentValidation;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.Interactors.Registration;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.PL.ViewModels.Models.Registration;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    [Route("register-employer")]
    public class CompanyRegisterController : BaseRegisterController<CompanyRegisterViewModel>
    {
        public const string AdsPricingAction = "RegisterPromotion";
        public const string StartPostOfferAndRegisterAction = "StartPostOfferAndRegister";

        private readonly IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor;
        private readonly CompanyPublishOfferAndRegistrationInteractor registrationWithPublishOfferInteractor;
        private readonly ILoginService<CompanyIdentity, CompanyProfile> loginService;
        private readonly IValidator<CompanyPublishOfferAndRegisterViewModel> validator;


        public CompanyRegisterController(
            IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor,
            CompanyPublishOfferAndRegistrationInteractor registrationWithPublishOfferInteractor,
            ILoginService<CompanyIdentity, CompanyProfile> loginService,
            IValidator<CompanyPublishOfferAndRegisterViewModel> validator)
        {
            this.registrationInteractor = registrationInteractor;
            this.registrationWithPublishOfferInteractor = registrationWithPublishOfferInteractor;
            this.loginService = loginService;
            this.validator = validator;
        }

        [Route("pricing")]
        public IActionResult RegisterPromotion()
        {
            return View();
        }

        [Route("post-ad")]
        public IActionResult StartPostOfferAndRegister()
        {
            var viewModel = new CompanyPublishOfferAndRegisterViewModel();
            return View(viewModel);
        }

        [Route("post-ad/{formDataTokenId}")]
        public async Task<IActionResult> StartPostOfferAndRegister(string formDataTokenId)
        {
            var viewModel = await registrationWithPublishOfferInteractor.GetPostFormViewModel(formDataTokenId);
            return View(viewModel);
        }

        [Route("post-ad/{formDataTokenId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> StartPostOfferAndRegister(
            CompanyPublishOfferAndRegisterViewModel registerData, string formDataTokenId)
        {
            return StartPostOfferAndRegister(registerData);
        }

        [Route("post-ad")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartPostOfferAndRegister(CompanyPublishOfferAndRegisterViewModel registerData)
        {
            var result = await validator.ValidateAsync(registerData);
            if (result.IsValid)
            {
                string tokenId = await registrationWithPublishOfferInteractor.SavePostFormViewModel(registerData);
                return RedirectToAction("VerifyRegistration", new { formDataTokenId = tokenId });
            }
            else
            {
                // TODO: remove after tests
                string tokenId = await registrationWithPublishOfferInteractor.SavePostFormViewModel(registerData);
                return RedirectToAction("VerifyRegistration", new { formDataTokenId = tokenId });
                //
                result.AddToModelState(this.ModelState);
            }

            return View(registerData);
        }

        [Route("verify/{formDataTokenId}")]
        public IActionResult VerifyRegistration(string formDataTokenId)
        {
            ViewData["FormDataTokenId"] = formDataTokenId;
            return View();
        }

        [Route("post-ad/confirm/{tokenId}")]
        public IActionResult TryConfirmEmployer(string tokenId)
        {
            return View();
        }

        [Route("post-ad/confirm/{tokenId}")]
        public async Task<IActionResult> TryConfirmOfferPaymentAndRegister(string tokenId)
        {
            await registrationInteractor.FinishRegistration(tokenId, HttpContext);
            return View();
        }

        //////
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
