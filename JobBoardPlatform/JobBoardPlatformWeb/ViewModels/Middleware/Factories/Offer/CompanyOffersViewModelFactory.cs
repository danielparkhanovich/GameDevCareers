using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer.CompanyBoard;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class CompanyOffersViewModelFactory : IFactory<CompanyOffersViewModel>
    {
        private readonly int profileId;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IMapper<JobOffer, CompanyOfferCardViewModel> offerToCardViewModel;


        public CompanyOffersViewModelFactory(int profileId, IRepository<JobOffer> offersRepository)
        {
            this.profileId = profileId;
            this.offersRepository = offersRepository;
            this.offerToCardViewModel = new JobOfferToCompanyOfferViewModelMapper();
        }

        private async Task<List<JobOffer>> GetOffers()
        {
            var offersSet = await offersRepository.GetAllSet();
            var offers = await offersSet.Where(offer => offer.CompanyProfileId == profileId)
                .OrderByDescending(offer => offer.CreatedAt)
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

        public async Task<CompanyOffersViewModel> Create()
        {
            var offers = await GetOffers();

            var display = new CompanyOffersViewModel();

            var offersDisplay = new List<CompanyOfferCardViewModel>();
            foreach (var offer in offers)
            {
                var offerCard = new CompanyOfferCardViewModel();

                var offerStateFactory = new OfferStateFactory();
                offerCard.IsVisible = offerStateFactory.IsOfferVisible(offer);
                offerCard.IsAvailable = offerStateFactory.IsOfferAvailable(offer);
                offerCard.StateType = offerStateFactory.GetOfferState(offer);

                offerToCardViewModel.Map(offer, offerCard);

                offersDisplay.Add(offerCard);
            }

            display.JobOffers = offersDisplay;

            return display;
        }
    }
}
