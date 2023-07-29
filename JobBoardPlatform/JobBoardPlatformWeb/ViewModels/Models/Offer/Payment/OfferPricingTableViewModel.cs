namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Payment
{
    public class OfferPricingTableViewModel
    {
        public List<string> Features { get; set; } = new List<string>();
        public List<OfferPricingPlanViewModel> Plans { get; set; } = new List<OfferPricingPlanViewModel>();
    }
}
