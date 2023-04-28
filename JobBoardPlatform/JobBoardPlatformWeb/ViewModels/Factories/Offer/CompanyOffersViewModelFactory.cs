using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class CompanyOffersViewModelFactory : IFactory<CompanyOffersViewModel>
    {
        private readonly int profileId;
        private readonly IRepository<JobOffer> offersRepository;


        public CompanyOffersViewModelFactory(int profileId, IRepository<JobOffer> offersRepository)
        {
            this.profileId = profileId;
            this.offersRepository = offersRepository;
        }

        public async Task<CompanyOffersViewModel> Create()
        {
            var offersLoader = new LoadCompanyOffers(offersRepository, profileId);
            var offers = await offersLoader.Load();

            var display = new CompanyOffersViewModel();

            var offersDisplay = new List<CompanyOfferCardViewModel>();
            foreach (var offer in offers)
            {
                var offerCard = await GetOfferCard(offer);

                offersDisplay.Add(offerCard);
            }

            display.JobOffers = offersDisplay;

            return display;
        }

        private async Task<CompanyOfferCardViewModel> GetOfferCard(JobOffer offer)
        {
            var offerCardFactory = new CompanyOfferViewModelFactory(offer);

            var offerCard = await offerCardFactory.Create();

            return offerCard;
        }
    }
}
