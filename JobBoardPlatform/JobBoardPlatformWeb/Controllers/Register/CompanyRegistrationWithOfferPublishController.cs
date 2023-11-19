using FluentValidation;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.Controllers.Presenters;
using JobBoardPlatform.PL.Filters;
using JobBoardPlatform.PL.Interactors.Notifications;
using JobBoardPlatform.PL.Interactors.Registration;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Registration;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    [Route("register-employer-offer")]
    public class CompanyRegistrationWithOfferPublishController : Controller
    {
        public const string AdsPricingAction = "RegisterPromotion";
        public const string StartPostOfferAndRegisterAction = "StartPostOfferAndRegister";

        private readonly IOfferPlanQueryExecutor plansQuery;
        private readonly IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor;
        private readonly EmailCompanyPublishOfferAndRegistrationInteractor formDataInteractor;
        private readonly IValidator<CompanyPublishOfferAndRegisterViewModel> validator;
        private readonly IValidator<CompanyRegisterViewModel> companyValidator;
        private readonly IViewRenderService viewRenderService;


        public CompanyRegistrationWithOfferPublishController(
            IOfferPlanQueryExecutor plansQuery,
            IRegistrationInteractor<CompanyRegisterViewModel> registrationInteractor,
            EmailCompanyPublishOfferAndRegistrationInteractor formDataInteractor,
            IValidator<CompanyPublishOfferAndRegisterViewModel> validator,
            IValidator<CompanyRegisterViewModel> companyValidator,
            IViewRenderService viewRenderService)
        {
            this.plansQuery = plansQuery;
            this.registrationInteractor = registrationInteractor;
            this.formDataInteractor = formDataInteractor;
            this.validator = validator;
            this.companyValidator = companyValidator;
            this.viewRenderService = viewRenderService;
        }

        [Route("pricing")]
        public async Task<IActionResult> RegisterPromotion()
        {
            var factory = new OfferPricingTableViewModelFactory(plansQuery);
            var viewModel = await factory.CreateAsync();
            return View(viewModel);
        }

        [Route("post-ad/{planType}/{formDataTokenId?}")]
        [TypeFilter(typeof(RedirectRegisteredCompanyFilter))]
        public async Task<IActionResult> StartPostOfferAndRegister(string planType, string? formDataTokenId = null)
        {
            var viewModel = new CompanyPublishOfferAndRegisterViewModel();
            if (!string.IsNullOrEmpty(formDataTokenId))
            {
                viewModel = await formDataInteractor.GetPostFormViewModelAsync(formDataTokenId);
            }
            int planId = GetPlanTypeId(planType);
            viewModel.EditOffer.OfferDetails.PlanId = planId;

            await SetPricingPlans(viewModel.EditOffer);
            return View(viewModel);
        }

        public IActionResult ReloadFormOnPlanChange(string planType, string? formDataTokenId = null)
        {
            return RedirectToAction(
                nameof(StartPostOfferAndRegister), 
                new { planType = planType, formDataTokenId = formDataTokenId });
        }

        [Route("post-ad/{planType}/{formDataTokenId?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartPostOfferAndRegister(
            CompanyPublishOfferAndRegisterViewModel registerData, string planType, string? formDataTokenId = null)
        {
            var result = await validator.ValidateAsync(registerData);
            if (result.IsValid)
            {
                if (!string.IsNullOrEmpty(formDataTokenId))
                {
                    await formDataInteractor.DeletePreviousSavedDataAsync(registerData, formDataTokenId);
                }
                string tokenId = await formDataInteractor.SavePostFormAsync(registerData);
                return RedirectToAction(nameof(VerifyRegistration), new { formDataTokenId = tokenId });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            await SetPricingPlans(registerData.EditOffer);
            return View(registerData);
        }

        [Route("verify/confirm/{confirmationTokenId}")]
        public async Task<IActionResult> TryConfirmRegistrationWithOfferPublish(string confirmationTokenId)
        {
            string formDataTokenId = await formDataInteractor.FinishRegistrationAsync(
                confirmationTokenId, HttpContext);
            return RedirectToAction(nameof(VerifyRegistration), new { formDataTokenId = formDataTokenId });
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
                return RedirectToAction(nameof(StartPostOfferAndRegister));
            }
        }

        [Route("verify/{formDataTokenId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyRegistration(CompanyVerifyPublishOfferAndRegistrationViewModel viewModel, string formDataTokenId)
        {
            viewRenderService.SetController(this);

            var userRegister = viewModel.UserRegister;

            var formData = await formDataInteractor.GetPostFormViewModelAsync(formDataTokenId);
            var companyData = formData.CompanyProfileData;
            userRegister.CompanyName = companyData.CompanyName!;

            var result = await companyValidator.ValidateAsync(userRegister);
            if (result.IsValid)
            {
                var redirect = await formDataInteractor.ProcessRegistrationAndRedirectAsync(
                    userRegister, formDataTokenId, TempData);
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

        [Route("post-ad/confirm/{tokenId}")]
        public async Task<IActionResult> TryConfirmOfferPaymentAndRegister(string tokenId)
        {
            await registrationInteractor.FinishRegistration(tokenId, HttpContext);
            return View();
        }

        private async Task<CompanyVerifyPublishOfferAndRegistrationViewModel> CreateCompanyVerifyViewModel(string formDataTokenId)
        {
            var viewModel = new CompanyVerifyPublishOfferAndRegistrationViewModel();
            viewModel.FormDataTokenId = formDataTokenId;

            var formData = await formDataInteractor.GetPostFormViewModelAsync(formDataTokenId);
            viewModel.PlanType = GetPlanType(formData.EditOffer.OfferDetails.PlanId);

            if (UserRolesUtils.IsUserCompany(User))
            {
                int profileId = UserSessionUtils.GetProfileId(User);
                var offer = await formDataInteractor.GetAddedOfferAsync(profileId);
                viewModel.PaymentForm = await GetPaymentFormViewModel(offer);
                viewModel.IsTokenConfirmed = true;
            }

            return viewModel;
        }

        private Task<OfferPaymentFormViewModel> GetPaymentFormViewModel(JobOffer offer)
        {
            var factory = new OfferPaymentFormViewModelFactory(plansQuery, offer);
            return factory.CreateAsync();
        }

        private async Task SetPricingPlans(EditOfferViewModel viewModel)
        {
            var pricingPlans = await (new OfferPricingTableViewModelFactory(plansQuery).CreateAsync());
            viewModel.PricingPlans = pricingPlans;
        }

        private int GetPlanTypeId(string plan)
        {
            var plans = Enum.GetValues(typeof(JobOfferPlanEnum))
                .Cast<JobOfferPlanEnum>().ToList();
            for (int i = 0; i < plans.Count; i++)
            {
                if (plan.ToLower() == plans[i].ToString().ToLower())
                {
                    return i + 1;
                }
            }
            return 2;
        }

        private string GetPlanType(int id)
        {
            var plans = Enum.GetValues(typeof(JobOfferPlanEnum))
                .Cast<JobOfferPlanEnum>().ToList();
            return plans[id - 1].ToString().ToLower();
        }
    }
}
