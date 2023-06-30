using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Route("manage-ads")]
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOffersPanelController : OfferCardsControllerBase
    {
        private readonly CompanyOffersSearcher offersSearcher;
        private readonly OfferQueryExecutor queryExecutor;


        public CompanyOffersPanelController(CompanyOffersSearcher offersSearcher,
            IOffersManager commandsExecutor,
            OfferQueryExecutor queryExecutor)
            : base(commandsExecutor)
        {
            this.offersSearcher = offersSearcher;
            this.queryExecutor = queryExecutor;
        }

        [Route("offers")]
        public async Task<IActionResult> Offers()
        {
            var containerFactory = new CompanyOffersContainerViewModelFactory(
                offersSearcher, GetSearchParams());
            var container = await containerFactory.CreateAsync();

            return View(container);
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var containerFactory = new CompanyOffersContainerViewModelFactory(
                offersSearcher, GetSearchParams());
            return containerFactory.CreateAsync();
        }

        private CompanyPanelOfferSearchParameters GetSearchParams()
        {
            var searchParamsFactory = new CompanyPanelOfferSearchParametersFactory();
            var searchParams = searchParamsFactory.GetSearchParams(Request);
            searchParams.CompanyProfileId = UserSessionUtils.GetProfileId(User);
            return searchParams;
        }

        protected override Task<JobOffer> GetLoadedOffer(int offerId)
        {
            return queryExecutor.GetOfferById(offerId);
        }

        protected override IContainerCard GetContainerCard(JobOffer offer)
        {
            var cardFactory = new CompanyOfferViewModelFactory();
            return cardFactory.CreateCard(offer);
        }

        public async Task<IActionResult> RequestPayment(int offerId)
        {
            // TODO: payment logic
            /*var offer = await offersRepository.Get(offerId);
            offer.IsPaid = true;
            offer.IsPublished = true;
            offer.PublishedAt = DateTime.Now;

            await offersRepository.Update(offer);*/

            return Redirect("Offers");
        }

        public async virtual Task<IActionResult> ProcessPayment(string paymentId, string orderId, string signature)
        {
            string message = "SUCCESS";
            return Json(new { Message = message });
        }
    }
}
