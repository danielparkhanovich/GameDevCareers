using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.EntityFrameworkCore;
using System;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IMapper<JobOffer, OfferCardViewModel> offerToCardViewModel;


        public OffersMainPageViewModelFactory(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
            this.offerToCardViewModel = new JobOfferToOfferViewModelMapper();
        }

        private async Task<List<JobOffer>> GetOffers()
        {
            var offerStateFactory = new OfferStateFactory();

            var offersSet = await offersRepository.GetAllSet();
            var filtered = offersSet.Where(offer => !offer.IsDeleted && offer.IsPaid && !offer.IsSuspended && !offer.IsShelved && offer.IsPublished);
            var offers = await filtered
                .OrderByDescending(offer => offer.PublishedAt)
                .Include(offer => offer.CompanyProfile)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .Include(offer => offer.ContactDetails)
                    .ThenInclude(details => details.ContactType)
                .ToListAsync();

            return offers;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            var offers = await GetOffers();

            var offersDisplay = new List<OfferCardViewModel>();

            foreach (var offer in offers)
            {
                var displayCard = new OfferCardViewModel();
                offerToCardViewModel.Map(offer, displayCard);

                offersDisplay.Add(displayCard);
            }

            var display = new OffersMainPageViewModel();

            display.Offers = offersDisplay;

            return display;
        }
    }
}
