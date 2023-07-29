using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment
{
    public class OfferPricingTableViewModelFactory : IViewModelAsyncFactory<OfferPricingTableViewModel>
    {
        private int selectedPlan;


        public OfferPricingTableViewModelFactory()
        {
        }

        public OfferPricingTableViewModelFactory(int selectedPlan)
        {
            this.selectedPlan = selectedPlan;
        }

        public Task<OfferPricingTableViewModel> CreateAsync()
        {
            var viewModel = new OfferPricingTableViewModel();
            viewModel.Features = GetFeatures();
            viewModel.Plans = GetPlans();
            if (selectedPlan != 0) 
            {
                var element = viewModel.Plans[selectedPlan - 1];
                viewModel.Plans = new List<OfferPricingPlanViewModel>() { element };
            }

            return Task.FromResult(viewModel);
        }

        private List<string> GetFeatures()
        {
            // TODO: replace by database data fetch
            return new List<string>()
            {
                "Number of publication days",
                "Number of locations",
                "Automatic bump-ups",
                "Ability to redirect applications",
                "Publish in category",
            };
        }

        private List<OfferPricingPlanViewModel> GetPlans()
        {
            // TODO: replace by database data fetch
            var plans = new List<OfferPricingPlanViewModel>
            {
                new OfferPricingPlanViewModel()
                {
                    OfferType = "Commission",
                    Price = "25",
                    Currency = "PLN",
                    FeatureValues = new List<string>()
                    {
                        "30 days", 
                        "1 location", 
                        "3 bump-up", 
                        "<i class=\"bi bi-check-lg text-success\" style=\"font-size: 1.5rem;\"></i>",
                        "Commissions",
                    }
                },
                new OfferPricingPlanViewModel()
                {
                    OfferType = "Indie",
                    Price = "50",
                    Currency = "PLN",
                    FeatureValues = new List<string>()
                    {
                        "30 days", 
                        "3 location", 
                        "3 bump-up", 
                        "<i class=\"bi bi-x-lg text-danger\" style=\"font-size: 1.5rem;\"></i>",
                        "Offers",
                    },
                    AvailableFreeSlots = 50
                },
                new OfferPricingPlanViewModel()
                {
                    OfferType = "AAA",
                    Price = "100",
                    Currency = "PLN",
                    FeatureValues = new List<string>()
                    {
                        "<strong>45 days</strong>",
                        "<strong>10 location</strong>",
                        "<strong>7 bump-up</strong>", 
                        "<i class=\"bi bi-check-lg text-success\" style=\"font-size: 1.5rem;\"></i>",
                        "Offers",
                    }
                }
            };
            return plans;
        }
    }
}
