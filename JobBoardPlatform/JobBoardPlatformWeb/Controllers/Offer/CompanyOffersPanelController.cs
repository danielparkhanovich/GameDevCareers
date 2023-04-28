using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOffersPanelController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;


        public CompanyOffersPanelController(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        public async virtual Task<IActionResult> Offers()
        {
            int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
            var viewModelFactory = new CompanyOffersViewModelFactory(profileId, offersRepository);

            var model = await viewModelFactory.Create();

            return View(model);
        }

        [HttpPost]
        public async virtual Task<IActionResult> ToggleOfferVisibility(int offerId, bool isVisible)
        {
            var offerLoader = new LoadCompanyOffer(offersRepository, offerId);
            var offer = await offerLoader.Load();

            offer.IsShelved = isVisible;
            await offersRepository.Update(offer);

            var offerCardFactory = new CompanyOfferViewModelFactory(offer);
            var offerCard = await offerCardFactory.Create();

            return PartialView("./JobOffers/_JobOfferCompanyView", offerCard);
        }

        [HttpPost]
        public async virtual Task<IActionResult> ToggleOfferCloseState(int offerId, bool isDeleted)
        {
            var offerLoader = new LoadCompanyOffer(offersRepository, offerId);
            var offer = await offerLoader.Load();

            offer.IsDeleted = isDeleted;
            await offersRepository.Update(offer);

            var offerCardFactory = new CompanyOfferViewModelFactory(offer);
            var offerCard = await offerCardFactory.Create();

            return PartialView("./JobOffers/_JobOfferCompanyView", offerCard);
        }

        public async Task<IActionResult> RequestPayment(int offerId)
        {
            // TODO: payment logic
            var offer = await offersRepository.Get(offerId);
            offer.IsPaid = true;
            offer.IsPublished = true;
            offer.PublishedAt = DateTime.Now;

            await offersRepository.Update(offer);

            return Redirect("Offers");
        }

        public async virtual Task<IActionResult> ProcessPayment(string paymentId, string orderId, string signature)
        {
            string message = "SUCCESS";
            return Json(new { Message = message });
        }
    }
}
