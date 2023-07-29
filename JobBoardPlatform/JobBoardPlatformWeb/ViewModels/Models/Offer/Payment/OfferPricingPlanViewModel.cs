namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Payment
{
    public class OfferPricingPlanViewModel
    {
        public string OfferType { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public int AvailableFreeSlots { get; set; }
        public List<string> FeatureValues { get; set; } = new List<string>();
    }
}
