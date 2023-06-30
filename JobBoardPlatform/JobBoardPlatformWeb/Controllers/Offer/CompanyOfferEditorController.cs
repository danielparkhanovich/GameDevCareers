using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOfferEditorController : Controller
    {
        private readonly OffersManager commandsExecutor;
        private readonly OfferQueryExecutor queryExecutor;


        public CompanyOfferEditorController(
            OffersManager commandsExecutor, OfferQueryExecutor queryExecutor)
        {
            this.commandsExecutor = commandsExecutor;
            this.queryExecutor = queryExecutor;
        }

        public IActionResult NewOffer()
        {
            return View();
        }

        [Route("edit-offer-{offerId}")]
        [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
        public async Task<IActionResult> NewOffer(int offerId)
        {
            var viewModel = await GetOfferDetailsViewModelAsync(offerId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-offer-{offerId}")]
        [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
        public async Task<IActionResult> EditOffer(int offerId, OfferDetailsViewModel viewModel)
        {
            return await TryAddOrModifyOffer(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOffer(OfferDetailsViewModel viewModel)
        {
            return await TryAddOrModifyOffer(viewModel);
        }

        private async Task<OfferDetailsViewModel> GetOfferDetailsViewModelAsync(int offerId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);
            var viewModelFactory = new OfferDetailsViewModelFactory();
            return viewModelFactory.Create(offer);
        }

        private async Task<IActionResult> TryAddOrModifyOffer(OfferDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int profileId = UserSessionUtils.GetProfileId(User);

                await commandsExecutor.AddAsync(profileId, viewModel);

                return RedirectToAction("Offers", "CompanyOffersPanel");
            }

            return View(viewModel);
        }
    }
}
