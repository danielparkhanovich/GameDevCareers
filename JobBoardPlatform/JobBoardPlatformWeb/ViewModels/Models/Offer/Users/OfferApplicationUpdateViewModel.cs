using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OfferApplicationUpdateViewModel : IApplicationForm
    {
        public int OfferId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IAttachedResume AttachedResume { get; set; }
        public string? AdditionalInformation { get; set; }
        public bool IsProcessingDataInFutureConsent { get; set; }
        public bool IsCustomConsent { get; set; }
    }
}
