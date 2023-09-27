using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment
{
    public class OfferPaymentFormViewModelFactory : IViewModelAsyncFactory<OfferPaymentFormViewModel>
    {
        private readonly JobOffer offer;
        private readonly IOfferPlanQueryExecutor plansQuery;


        public OfferPaymentFormViewModelFactory(IOfferPlanQueryExecutor plansQuery, JobOffer offer)
        {
            this.plansQuery = plansQuery;
            this.offer = offer;
        }

        public async Task<OfferPaymentFormViewModel> CreateAsync()
        {
            var viewModel = new OfferPaymentFormViewModel();
            viewModel.OfferCard = GetOfferCard(offer);
            viewModel.SelectedPlan = await GetSelectedPlan(offer.PlanId);
            viewModel.OfferId = offer.Id;
            return viewModel;
        }

        private OfferCardViewModel GetOfferCard(JobOffer offer)
        {
            var cardFactory = new OfferCardViewModelFactory();
            var card = cardFactory.CreateCard(offer);
            return card as OfferCardViewModel;
        }

        private async Task<OfferPricingTableViewModel> GetSelectedPlan(int selectedPlanId)
        {
            var factory = new OfferPricingTableViewModelFactory(plansQuery, selectedPlanId);
            return await factory.CreateAsync();
        }
    }
}
