using FluentValidation;
using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Controllers.Presenters;
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
        private readonly IOfferManager offersManager;
        private readonly IOfferPlanQueryExecutor plansQuery;
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IValidator<OfferData> validator;
        private readonly IPaymentInteractor paymentInteractor;


        public CompanyOfferPaymentController(
            IOfferManager offersManager,
            IOfferPlanQueryExecutor plansQuery,
            IOfferQueryExecutor queryExecutor,
            IValidator<OfferData> validator,
            IPaymentInteractor paymentInteractor)
        {
            this.offersManager = offersManager;
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

            var viewModel = await GetViewModel(offer);
            return View(viewModel);
        }

        [Route("check-out-{offerId}")]
        public async Task<IActionResult> Checkout(int offerId)
        {
            if (await plansQuery.IsFreePlan(offerId))
            {
                await plansQuery.RemoveFreeSlot(offerId);
                await offersManager.PassPaymentAsync(offerId);
                return RedirectToAction(nameof(Confirmation), new { offerId = offerId, checkoutSessionId = "free" });
            }

            await paymentInteractor.ProcessCheckout(offerId);
            return new StatusCodeResult(303);
        }

        [Route("check-out-{offerId}/{checkoutSessionId}")]
        public async Task<IActionResult> Confirmation(int offerId, string checkoutSessionId)
        {
            var offer = await offersManager.GetAsync(offerId);
            if (!offer.IsPaid)
            {
                try
                {
                    await paymentInteractor.ConfirmCheckout(offerId, checkoutSessionId);
                }
                catch (StripeException)
                {
                    return RedirectToAction(nameof(Rejected), new { offerId = offerId });
                }
            }

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
