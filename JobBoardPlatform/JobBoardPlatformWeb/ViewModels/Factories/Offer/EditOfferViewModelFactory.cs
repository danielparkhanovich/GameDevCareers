using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class EditOfferViewModelFactory : IViewModelAsyncFactory<EditOfferViewModel>
    {
        private readonly IOfferPlanQueryExecutor offerPlansQuery;


        public EditOfferViewModelFactory(IOfferPlanQueryExecutor offerPlansQuery)
        {
            this.offerPlansQuery = offerPlansQuery;
        }

        public async Task<EditOfferViewModel> CreateAsync()
        {
            var viewModel = new EditOfferViewModel();
            await SetPricingPlans(viewModel);
            return viewModel;
        }

        private async Task SetPricingPlans(EditOfferViewModel viewModel)
        {
            var pricingPlans = await (new OfferPricingTableViewModelFactory(offerPlansQuery).CreateAsync());
            viewModel.PricingPlans = pricingPlans;
        }
    }
}
