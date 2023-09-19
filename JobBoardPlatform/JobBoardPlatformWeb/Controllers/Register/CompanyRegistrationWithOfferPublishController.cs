using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.PL.Aspects.DataValidators;
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
        private readonly IRegistrationInteractor<CompanyRegisterViewModel> interactor;
        private readonly EmailCompanyPublishOfferAndRegistrationInteractor extendedInteractor;
        private readonly IValidator<CompanyPublishOfferAndRegisterViewModel> validator;
        private readonly IValidator<CompanyRegisterViewModel> companyValidator;


        public CompanyRegistrationWithOfferPublishController(
            IOfferPlanQueryExecutor plansQuery,
            IRegistrationInteractor<CompanyRegisterViewModel> interactor,
            EmailCompanyPublishOfferAndRegistrationInteractor extendedInteractor,
            IValidator<CompanyPublishOfferAndRegisterViewModel> validator,
            IValidator<CompanyRegisterViewModel> companyValidator)
        {
            this.plansQuery = plansQuery;
            this.interactor = interactor;
            this.extendedInteractor = extendedInteractor;
            this.validator = validator;
            this.companyValidator = companyValidator;
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
            ICompanyProfileAndNewOfferData viewModel;
            if (string.IsNullOrEmpty(formDataTokenId))
            {
                viewModel = new CompanyPublishOfferAndRegisterViewModel();
            }
            else
            {
                viewModel = await extendedInteractor.GetPostFormViewModelAsync(formDataTokenId);
            }
            int planId = GetPlanTypeId(planType);
            viewModel.OfferData.PlanId = planId;

            await SetPricingPlans((viewModel as CompanyPublishOfferAndRegisterViewModel).EditOffer);
            return View(viewModel);
        }

        public IActionResult ReloadFormOnPlanChange(string planType, string? formDataTokenId = null)
        {
            return RedirectToAction(
                "StartPostOfferAndRegister", 
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
                    await extendedInteractor.DeletePreviousSavedDataAsync(registerData, formDataTokenId);
                }
                string tokenId = await extendedInteractor.SavePostFormViewModelAsync(registerData);
                return RedirectToAction("VerifyRegistration", new { formDataTokenId = tokenId });
            }
            else
            {
                // TODO: remove after tests
                string tokenId = await extendedInteractor.SavePostFormViewModelAsync(registerData);
                return RedirectToAction("VerifyRegistration", new { formDataTokenId = tokenId });
                //
                result.AddToModelState(this.ModelState);
            }

            return View(registerData);
        }

        [Route("verify/confirm/{confirmationTokenId}")]
        public async Task<IActionResult> TryConfirmRegistrationWithOfferPublish(string confirmationTokenId)
        {
            string formDataTokenId = await extendedInteractor.FinishRegistrationAsync(
                confirmationTokenId, HttpContext);
            return RedirectToAction("VerifyRegistration", new { formDataTokenId = formDataTokenId });
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

            var formData = await extendedInteractor.GetPostFormViewModelAsync(formDataTokenId);
            var companyData = formData.CompanyProfileData;
            userRegister.CompanyName = companyData.CompanyName!;

            var result = await companyValidator.ValidateAsync(userRegister);
            if (result.IsValid)
            {
                var redirect = await extendedInteractor.ProcessRegistrationAndRedirectAsync(
                    userRegister, formDataTokenId);
                NotificationsManager.Instance.SetActionDoneNotification(
                    NotificationsManager.RegisterSection,
                    $"Check your email inbox {userRegister.Email} for a confirmation link to complete your registration.",
                    TempData);
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
            await interactor.FinishRegistration(tokenId, HttpContext);
            return View();
        }

        private async Task<CompanyVerifyPublishOfferAndRegistrationViewModel> CreateCompanyVerifyViewModel(string formDataTokenId)
        {
            var viewModel = new CompanyVerifyPublishOfferAndRegistrationViewModel();
            viewModel.FormDataTokenId = formDataTokenId;

            var formData = await extendedInteractor.GetPostFormViewModelAsync(formDataTokenId);
            viewModel.PlanType = GetPlanType(formData.OfferData.PlanId);

            if (UserSessionUtils.IsLoggedIn(User))
            {
                int profileId = UserSessionUtils.GetProfileId(User);
                var offer = await extendedInteractor.GetAddedOfferAsync(profileId);
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
