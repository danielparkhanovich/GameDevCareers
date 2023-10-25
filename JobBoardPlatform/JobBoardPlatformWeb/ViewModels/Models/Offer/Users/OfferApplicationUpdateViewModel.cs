using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OfferApplicationUpdateViewModel : ApplicationForm
    {
        public bool IsProcessingDataInFutureConsent { get; set; }
        public bool IsCustomConsent { get; set; }
    }
}
