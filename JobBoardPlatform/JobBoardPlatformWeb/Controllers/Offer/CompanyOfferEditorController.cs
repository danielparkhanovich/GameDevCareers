using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOfferEditorController : Controller
    {
        private readonly OfferCommandsExecutor commandsExecutor;


        public CompanyOfferEditorController(OfferCommandsExecutor commandsExecutor)
        {
            this.commandsExecutor = commandsExecutor;
        }

        public IActionResult NewOffer()
        {
            return View();
        }

        [Route("edit-offer-{offerId}")]
        [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
        public async Task<IActionResult> NewOffer(int offerId)
        {
            var viewModelFactory = new OfferDetailsViewModelFactory();
            var viewModel = await viewModelFactory.Create();

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
