using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    [Route("offer")]
    public class CompanyOfferEditorController : Controller
    {
        public const string AddNewOfferAction = "AddNewOffer";
        public const string EditOfferAction = "Editor";

        private readonly IOfferManager offersManager;
        private readonly IValidator<IOfferData> validator;


        public CompanyOfferEditorController(
            IOfferManager offersManager, 
            IValidator<IOfferData> validator)
        {
            this.offersManager = offersManager;
            this.validator = validator;
        }

        [Route("new", Order = 1)]
        public IActionResult Editor()
        {
            var viewModel = new EditOfferViewModel();
            return View(viewModel);
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewOffer(EditOfferViewModel viewModel)
        {
            return await TryAddNewOfferAsync(viewModel);
        }

        [Route("edit-{offerId}", Order = 0)]
        [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
        public async Task<IActionResult> Editor(int offerId)
        {
            var viewModel = new EditOfferViewModel();
            viewModel.OfferDetails = await GetOfferDetailsViewModelAsync(offerId);
            return View(viewModel);
        }

        [HttpPost("edit-{offerId}")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
        public async Task<IActionResult> Editor(int offerId, EditOfferViewModel viewModel)
        {
            viewModel.OfferDetails.OfferId = offerId;
            return await TryUpdateOfferAsync(viewModel);
        }

        private async Task<OfferDetailsViewModel> GetOfferDetailsViewModelAsync(int offerId)
        {
            var offer = await offersManager.GetAsync(offerId);
            var viewModelFactory = new OfferDetailsViewModelFactory();
            return viewModelFactory.Create(offer);
        }

        private async Task<IActionResult> TryAddNewOfferAsync(EditOfferViewModel viewModel)
        {
            var result = await validator.ValidateAsync(viewModel.OfferDetails);
            if (result.IsValid)
            {
                int profileId = UserSessionUtils.GetProfileId(User);
                await offersManager.AddAsync(profileId, viewModel.OfferDetails);
                return RedirectToAction("Offers", "CompanyOffersPanel");
            }
            else
            {
                result.AddToModelState(this.ModelState, nameof(viewModel.OfferDetails));
            }

            return View(viewModel);
        }

        private async Task<IActionResult> TryUpdateOfferAsync(EditOfferViewModel viewModel)
        {
            var result = await validator.ValidateAsync(viewModel.OfferDetails);
            if (result.IsValid)
            {
                await offersManager.UpdateAsync(viewModel.OfferDetails);
                return RedirectToAction("Offers", "CompanyOffersPanel");
            }
            else
            {
                result.AddToModelState(this.ModelState, nameof(viewModel.OfferDetails));
            }

            return View(viewModel);
        }
    }
}
