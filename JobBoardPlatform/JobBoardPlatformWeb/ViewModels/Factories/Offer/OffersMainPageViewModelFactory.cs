using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
        private readonly IRepository<JobOffer> offersRepository;


        public OffersMainPageViewModelFactory(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            var offersLoader = new LoadActualOffersPage(offersRepository, 1, 10);
            var offers = await offersLoader.Load();

            var offersDisplay = new List<OfferCardViewModel>();

            foreach (var offer in offers)
            {
                var offerCardFactory = new OfferCardViewModelFactory(offer);
                var offerCard = await offerCardFactory.Create();

                offersDisplay.Add(offerCard);
            }

            var display = new OffersMainPageViewModel();

            display.Offers = offersDisplay;

            return display;
        }
    }
}
