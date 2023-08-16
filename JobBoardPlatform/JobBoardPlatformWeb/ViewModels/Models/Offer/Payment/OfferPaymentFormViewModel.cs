using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Payment
{
    public class OfferPaymentFormViewModel
    {
        public int OfferId { get; set; }
        public OfferCardViewModel OfferCard { get; set; } = new OfferCardViewModel();
        public OfferPricingTableViewModel SelectedPlan { get; set; } = new OfferPricingTableViewModel();
    }
}
