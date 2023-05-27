using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    public abstract class OfferCardsControllerBase : CardsControllerBase
    {
        public const string ToggleVisibilityAction = "ToggleOfferVisibilityState";
        public const string ToggleSuspendAction = "ToggleOfferSuspendState";
        public const string DeleteAction = "CloseOffer";
        public const string PassPaymentAction = "PassPaymentOffer";

        private readonly OfferCommandsExecutor commandsExecutor;


        public OfferCardsControllerBase(OfferCommandsExecutor commandsExecutor)
        {
            this.commandsExecutor = commandsExecutor;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleOfferVisibilityState(int offerId, bool flag)
        {
            await commandsExecutor.ShelveAsync(offerId, flag);
            var offerCard = await GetOfferCardPartialView(offerId);
            return offerCard;
        }

        [HttpPost]
        public async Task<IActionResult> CloseOffer(int offerId)
        {
            await commandsExecutor.DeleteAsync(offerId);
            return new EmptyResult();
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
        public async Task<IActionResult> ToggleOfferSuspendState(int offerId, bool flag)
        {
            await commandsExecutor.SuspendAsync(offerId, flag);
            var offerCard = await GetOfferCardPartialView(offerId);
            return offerCard;
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
        public async Task<IActionResult> PassPaymentOffer(int offerId)
        {
            await commandsExecutor.PassPaymentAsync(offerId);
            var offerCard = await GetOfferCardPartialView(offerId);
            return offerCard;
        }

        protected override abstract Task<CardsContainerViewModel> GetContainer();

        protected abstract Task<JobOffer> GetLoadedOffer(int offerId);

        protected abstract IContainerCard GetContainerCard(JobOffer offer);

        private async Task<PartialViewResult> GetOfferCardPartialView(int offerId)
        {
            var offer = await GetLoadedOffer(offerId);
            var offerCard = GetContainerCard(offer);
            return PartialView(offerCard.PartialView, offerCard);
        }
    }
}
