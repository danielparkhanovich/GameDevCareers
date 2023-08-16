
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class EditOfferViewModel
    {
        public OfferPricingTableViewModel PricingPlans = new OfferPricingTableViewModel();
        public EditOfferDisplayViewModel Display { get; set; } = new EditOfferDisplayViewModel();
        public OfferDataViewModel OfferDetails { get; set; } = new OfferDataViewModel();
    }
}
