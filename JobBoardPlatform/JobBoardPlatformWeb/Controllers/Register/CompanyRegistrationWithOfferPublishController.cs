using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.Filters;
using JobBoardPlatform.PL.Interactors.Registration;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.PL.ViewModels.Models.Registration;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    [Route("register-employer-offer")]
    [TypeFilter(typeof(RedirectRegisteredCompanyFilter))]
    public class CompanyRegistrationWithOfferPublishController : Controller
    {
        public const string AdsPricingAction = "RegisterPromotion";
        public const string StartPostOfferAndRegisterAction = "StartPostOfferAndRegister";

        private readonly IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor;
        private readonly EmailCompanyPublishOfferAndRegistrationInteractor registrationWithPublishOfferInteractor;
        private readonly IValidator<CompanyPublishOfferAndRegisterViewModel> validator;
        private readonly IValidator<CompanyRegisterViewModel> companyRegisterValidator;


        public CompanyRegistrationWithOfferPublishController(
            IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor,
            EmailCompanyPublishOfferAndRegistrationInteractor registrationWithPublishOfferInteractor,
            IValidator<CompanyPublishOfferAndRegisterViewModel> validator,
            IValidator<CompanyRegisterViewModel> companyRegisterValidator)
        {
            this.registrationInteractor = registrationInteractor;
            this.registrationWithPublishOfferInteractor = registrationWithPublishOfferInteractor;
            this.validator = validator;
            this.companyRegisterValidator = companyRegisterValidator;
        }

        [Route("pricing")]
        public async Task<IActionResult> RegisterPromotion()
        {
            var factory = new OfferPricingTableViewModelFactory();
            var viewModel = await factory.CreateAsync();
            return View(viewModel);
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
        public async Task<IActionResult> VerifyRegistration(string formDataTokenId)
        {
            try
            {
                var viewModel = await CreateCompanyVerifyViewModel(formDataTokenId);
                return View(viewModel);
            }
            catch (TokenValidationException e)
            {
                return RedirectToAction("StartPostOfferAndRegister");
            }
        }

        [Route("verify/{formDataTokenId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyRegistration(CompanyVerifyPublishOfferAndRegistrationViewModel viewModel, string formDataTokenId)
        {
            var userRegister = viewModel.UserRegister;

            var formData = await registrationWithPublishOfferInteractor.GetPostFormViewModel(formDataTokenId);
            var companyData = formData.CompanyProfileData;
            userRegister.CompanyName = companyData.CompanyName!;

            var result = await companyRegisterValidator.ValidateAsync(userRegister);
            if (result.IsValid)
            {
                var redirect = await registrationWithPublishOfferInteractor.ProcessRegistrationAndRedirect(
                    userRegister, formDataTokenId);
                return RedirectToAction(redirect.ActionName, redirect.Data);
            }
            else
            {
                result.AddToModelState(this.ModelState, nameof(viewModel.UserRegister));
            }

            if (ModelState.ErrorCount == 0)
            {
                viewModel.UserRegister = new CompanyRegisterViewModel();
            }
            return View(viewModel);
        }

        [Route("verify/confirm/{confirmationTokenId}")]
        public async Task<IActionResult> TryConfirmRegistrationWithOfferPublish(string confirmationTokenId)
        {
            string formDataTokenId = await registrationWithPublishOfferInteractor.FinishRegistration(
                confirmationTokenId, HttpContext);
            return RedirectToAction("VerifyRegistration", new { formDataTokenId = formDataTokenId });
        }

        [Route("post-ad/confirm/{tokenId}")]
        public async Task<IActionResult> TryConfirmOfferPaymentAndRegister(string tokenId)
        {
            await registrationInteractor.FinishRegistration(tokenId, HttpContext);
            return View();
        }

        private async Task<CompanyVerifyPublishOfferAndRegistrationViewModel> CreateCompanyVerifyViewModel(string formDataTokenId)
        {
            var formData = await registrationWithPublishOfferInteractor.GetPostFormViewModel(formDataTokenId);
            var viewModel = new CompanyVerifyPublishOfferAndRegistrationViewModel();
            viewModel.FormDataTokenId = formDataTokenId;
            if (await registrationWithPublishOfferInteractor.IsFormDataConfirmed(formDataTokenId))
            {
                viewModel.OfferCard = CreateOfferCard(formData.OfferData, formData.CompanyProfileData);
            }
            return viewModel;
        }

        private IContainerCard CreateOfferCard(INewOfferData offerData, ICompanyProfileData companyProfile)
        {
            var offerCardFactory = new OfferCardViewModelFromOfferFormFactory(offerData, companyProfile);
            return offerCardFactory.Create();
        }
    }
}
