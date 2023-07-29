using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOfferEditorController : Controller
    {
        private readonly IOffersManager commandsExecutor;
        private readonly OfferQueryExecutor queryExecutor;
        private readonly IValidator<INewOfferData> validator;


        public CompanyOfferEditorController(
            IOffersManager commandsExecutor, 
            OfferQueryExecutor queryExecutor,
            IValidator<INewOfferData> validator)
        {
            this.commandsExecutor = commandsExecutor;
            this.queryExecutor = queryExecutor;
            this.validator = validator;
        }

        public IActionResult NewOffer()
        {
            var viewModel = new EditOfferViewModel();
            return View(viewModel);
        }

        [Route("edit-offer-{offerId}")]
        [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
        public async Task<IActionResult> NewOffer(int offerId)
        {
            var viewModel = new EditOfferViewModel();
            viewModel.OfferDetails = await GetOfferDetailsViewModelAsync(offerId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-offer-{offerId}")]
        [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
        public async Task<IActionResult> NewOffer(int offerId, EditOfferViewModel viewModel)
        {
            viewModel.OfferDetails.OfferId = offerId;
            return await TryAddOrModifyOffer(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOffer(EditOfferViewModel viewModel)
        {
            return await TryAddOrModifyOffer(viewModel);
        }

        private async Task<OfferDetailsViewModel> GetOfferDetailsViewModelAsync(int offerId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);
            var viewModelFactory = new OfferDetailsViewModelFactory();
            return viewModelFactory.Create(offer);
        }

        private async Task<IActionResult> TryAddOrModifyOffer(EditOfferViewModel viewModel)
        {
            var result = await validator.ValidateAsync(viewModel.OfferDetails);
            if (result.IsValid)
            {
                int profileId = UserSessionUtils.GetProfileId(User);

                await commandsExecutor.AddAsync(profileId, viewModel.OfferDetails);

                return RedirectToAction("Offers", "CompanyOffersPanel");
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            return View(viewModel);
        }
    }
}
