using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment
{
    public class OfferPaymentFormViewModelFactory : IViewModelAsyncFactory<OfferPaymentFormViewModel>
    {
        private readonly JobOffer offer;


        public OfferPaymentFormViewModelFactory(JobOffer offer)
        {
            this.offer = offer;
        }

        public async Task<OfferPaymentFormViewModel> CreateAsync()
        {
            var viewModel = new OfferPaymentFormViewModel();
            viewModel.OfferCard = GetOfferCard(offer);
            viewModel.SelectedPlan = await GetSelectedPlan();
            viewModel.OfferId = offer.Id;
            return viewModel;
        }

        private OfferCardViewModel GetOfferCard(JobOffer offer)
        {
            var cardFactory = new OfferCardViewModelFactory();
            var card = cardFactory.CreateCard(offer);
            return card as OfferCardViewModel;
        }

        private async Task<OfferPricingTableViewModel> GetSelectedPlan()
        {
            var factory = new OfferPricingTableViewModelFactory(3);
            return await factory.CreateAsync();
        }
    }
}
