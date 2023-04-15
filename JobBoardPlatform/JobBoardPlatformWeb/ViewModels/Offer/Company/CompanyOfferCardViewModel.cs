using JobBoardPlatform.PL.ViewModels.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.OfferViewModels.Company
{
    public class CompanyOfferCardViewModel
    {
        public string MainTechnology { get; set; } = string.Empty;
        public string ContactType { get; set; } = string.Empty;
        public string? ContactAddress { get; set; }
        public bool IsPublished { get; set; }
        public OfferCardViewModel CardDisplay { get; set; }
    }
}
