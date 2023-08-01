using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class EditOfferViewModel
    {
        public EditOfferDisplayViewModel Display { get; set; } = new EditOfferDisplayViewModel();
        public IOfferData OfferDetails { get; set; } = new OfferDetailsViewModel();
    }
}
