using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer.CompanyBoard;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            var offer = await GetJobOffer(offerId);
            offer.IsShelved = isVisible;
            await offersRepository.Update(offer);

            var offerToCardViewModel = new JobOfferToCompanyOfferViewModelMapper();
            var offerCard = new CompanyOfferCardViewModel();
            offerToCardViewModel.Map(offer, offerCard);
            var offerStateFactory = new OfferStateFactory();
            offerCard.IsVisible = offerStateFactory.IsOfferVisible(offer);
            offerCard.IsAvailable = offerStateFactory.IsOfferAvailable(offer);
            offerCard.StateType = offerStateFactory.GetOfferState(offer);

            return PartialView("./JobOffers/_JobOfferCompanyView", offerCard);
        }

        [HttpPost]
        public async virtual Task<IActionResult> ToggleOfferCloseState(int offerId, bool isDeleted)
        {
            var offer = await GetJobOffer(offerId);
            offer.IsDeleted = isDeleted;
            await offersRepository.Update(offer);

            var offerToCardViewModel = new JobOfferToCompanyOfferViewModelMapper();
            var offerCard = new CompanyOfferCardViewModel();
            offerToCardViewModel.Map(offer, offerCard);
            var offerStateFactory = new OfferStateFactory();
            offerCard.IsVisible = offerStateFactory.IsOfferVisible(offer);
            offerCard.IsAvailable = offerStateFactory.IsOfferAvailable(offer);
            offerCard.StateType = offerStateFactory.GetOfferState(offer);

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

        private async Task<JobOffer> GetJobOffer(int offerId)
        {
            var offersSet = await offersRepository.GetAllSet();
            var offer = await offersSet
                .Where(offer => offer.Id == offerId) // replace Id with the actual property name of your offer's ID
                .Include(offer => offer.CompanyProfile)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .Include(offer => offer.ContactDetails)
                    .ThenInclude(details => details.ContactType)
                .FirstOrDefaultAsync();

            return offer;
        }
    }
}
