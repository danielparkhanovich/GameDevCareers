﻿using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.Interactors.Payment;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
    [Route("payment")]
    public class CompanyOfferPaymentController : Controller
    {
        private readonly IOffersManager commandsExecutor;
        private readonly OfferQueryExecutor queryExecutor;
        private readonly IValidator<INewOfferData> validator;
        private readonly IPaymentInteractor paymentInteractor;


        public CompanyOfferPaymentController(
            IOffersManager commandsExecutor, 
            OfferQueryExecutor queryExecutor,
            IValidator<INewOfferData> validator,
            IPaymentInteractor paymentInteractor)
        {
            this.commandsExecutor = commandsExecutor;
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
        public async Task<IActionResult> CheckOut(int offerId)
        {
            await paymentInteractor.ProcessCheckout(offerId);
            return new StatusCodeResult(303);
        }

        [Route("check-out-{offerId}/{checkoutSessionId}")]
        public async Task<IActionResult> Confirmation(int offerId, string checkoutSessionId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);
            var viewModel = GetOfferCard(offer);
            return View(viewModel);
        }

        private IActionResult RedirectToOffersPanel()
        {
            string controller = ControllerUtils.GetControllerName(typeof(CompanyOffersPanelController));
            return RedirectToAction("Offers", controller);
        }

        private async Task<OfferPaymentFormViewModel> GetViewModel(JobOffer offer)
        {
            var viewModel = new OfferPaymentFormViewModel();
            viewModel.OfferCard = GetOfferCard(offer);
            viewModel.SelectedPlan = await GetSelectedPlan();
            viewModel.OfferId = offer.Id;
            return viewModel;
        }

        private OfferCardViewModel GetOfferCard(JobOffer offer)
        {
            var cardFactory = new OfferCardViewModelFactory();
            var card = cardFactory.CreateCard(offer);
            return card as OfferCardViewModel;
        }

        private async Task<OfferPricingTableViewModel> GetSelectedPlan()
        {
            var factory = new OfferPricingTableViewModelFactory(3);
            return await factory.CreateAsync();
        }
    }
}