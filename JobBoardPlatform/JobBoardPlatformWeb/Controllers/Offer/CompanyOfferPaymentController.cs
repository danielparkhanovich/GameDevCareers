using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.Interactors.Notifications;
using JobBoardPlatform.PL.Interactors.Payment;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
    [Route("payment")]
    public class CompanyOfferPaymentController : Controller
    {
        private readonly IOfferManager offerManager;
        private readonly IOfferPlanQueryExecutor plansQuery;
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IValidator<IOfferData> validator;
        private readonly IPaymentInteractor paymentInteractor;


        public CompanyOfferPaymentController(
            IOfferManager offerManager,
            IOfferPlanQueryExecutor plansQuery,
            IOfferQueryExecutor queryExecutor,
            IValidator<IOfferData> validator,
            IPaymentInteractor paymentInteractor)
        {
            this.offerManager = offerManager;
            this.plansQuery = plansQuery;
            this.queryExecutor = queryExecutor;
            this.validator = validator;
            this.paymentInteractor = paymentInteractor;
        }

        [Route("offer-{offerId}")]
        public async Task<IActionResult> Payment(int offerId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);
            if (offer.IsPaid)
            {
                return RedirectToOffersPanel();
            }

            TempData[CompanyOfferEditorController.NextActionTempData] = RouteData.Values["action"].ToString();
            TempData[CompanyOfferEditorController.NextControllerTempData] = RouteData.Values["controller"].ToString();
            TempData[CompanyOfferEditorController.OfferIdTempData] = RouteData.Values["offerId"].ToString();

            var viewModel = await GetViewModel(offer);
            return View(viewModel);
        }

        [Route("check-out-{offerId}")]
        public async Task<IActionResult> Checkout(int offerId)
        {
            await paymentInteractor.ProcessCheckout(offerId);
            return new StatusCodeResult(303);
        }

        [Route("check-out-{offerId}/{checkoutSessionId}")]
        public async Task<IActionResult> Confirmation(int offerId, string checkoutSessionId)
        {
            try
            {
                await paymentInteractor.ConfirmCheckout(offerId, checkoutSessionId);
            }
            catch (StripeException)
            {
                RedirectToAction("Rejected", new { offerId = offerId });
            }

            var offer = await queryExecutor.GetOfferById(offerId);
            var viewModel = await GetViewModel(offer);
            return View(viewModel.OfferCard);
        }

        [Route("check-out-{offerId}/rejected")]
        public IActionResult Rejected(int offerId)
        {
            NotificationsManager.Instance.SetErrorNotification(
                NotificationsManager.PaymentSection, "Something went wrong, please try again", TempData);
            return RedirectToAction("Payment", new { offerId = offerId });
        }

        private IActionResult RedirectToOffersPanel()
        {
            string controller = ControllerUtils.GetControllerName(typeof(CompanyOffersPanelController));
            return RedirectToAction("Offers", controller);
        }

        private Task<OfferPaymentFormViewModel> GetViewModel(JobOffer offer)
        {
            var factory = new OfferPaymentFormViewModelFactory(plansQuery, offer);
            return factory.CreateAsync();
        }
    }
}
