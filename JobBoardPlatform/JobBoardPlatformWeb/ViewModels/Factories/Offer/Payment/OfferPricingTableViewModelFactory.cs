using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment
{
    public class OfferPricingTableViewModelFactory : IViewModelAsyncFactory<OfferPricingTableViewModel>
    {
        private readonly int selectedPlan;
        private readonly IOfferPlanQueryExecutor plansQuery;


        public OfferPricingTableViewModelFactory(IOfferPlanQueryExecutor plansQuery)
        {
            this.plansQuery = plansQuery;
        }

        public OfferPricingTableViewModelFactory(IOfferPlanQueryExecutor plansQuery, int selectedPlan)
        {
            this.plansQuery = plansQuery;
            this.selectedPlan = selectedPlan;
        }

        public async Task<OfferPricingTableViewModel> CreateAsync()
        {
            var viewModel = new OfferPricingTableViewModel();
            viewModel.Features = GetFeatures();
            viewModel.Plans = await GetPlansAsync();
            if (selectedPlan != 0) 
            {
                var element = viewModel.Plans[selectedPlan - 1];
                viewModel.Plans = new List<OfferPricingPlanViewModel>() { element };
            }

            return viewModel;
        }

        private List<string> GetFeatures()
        {
            return new List<string>()
            {
                "Number of publication days",
                "Number of locations",
                "Automatic bump-ups",
                "Ability to redirect applications",
                "Publish in category",
            };
        }

        private async Task<List<OfferPricingPlanViewModel>> GetPlansAsync()
        {
            var viewModels = new List<OfferPricingPlanViewModel>();

            var plans = await plansQuery.GetAllAsync();
            foreach (var plan in plans) 
            {
                var viewModel = GetOfferPricingPlanViewModel(plan);
                viewModels.Add(viewModel);
            }
            return viewModels;
        }

        private OfferPricingPlanViewModel GetOfferPricingPlanViewModel(JobOfferPlan plan)
        {
            var viewModel = new OfferPricingPlanViewModel();
            viewModel.OfferType = plan.Name.Type;
            viewModel.Price = plan.PriceInPLN.ToString();
            viewModel.Currency = "PLN";
            viewModel.AvailableFreeSlots = plan.FreeSlotsCount;
            MapFeatureValues(plan, viewModel);
            return viewModel;
        }

        private void MapFeatureValues(JobOfferPlan from, OfferPricingPlanViewModel to)
        {
            to.FeatureValues = new List<string>()
            {
                $"{from.PublicationDaysCount} days",
                $"{from.EmploymentLocationsCount} locations",
                $"{from.OfferRefreshesCount} bump-ups",
                $"{GetOptionIsInludedOrNotHtml(from.IsAbleToRedirectApplications)}",
                $"{from.Category.Type}"
            };
        }

        private string GetOptionIsInludedOrNotHtml(bool isIncluded)
        {
            if (isIncluded)
            {
                return GetOptionIncludedHtml();
            }
            else
            {
                return GetOptionNotIncludedHtml();
            }
        }

        private string GetOptionIncludedHtml()
        {
            return "<i class=\"bi bi-check-lg text-success\" style=\"font-size: 1.5rem;\"></i>";
        }

        private string GetOptionNotIncludedHtml()
        {
            return "<i class=\"bi bi-x-lg text-danger\" style=\"font-size: 1.5rem;\"></i>";
        }
    }
}
