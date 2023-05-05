using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class CompanyApplicationsViewModel
    {
        public OfferCardViewModel OfferCard { get; set; }
        public ContainerCardsViewModel? Applications { get; set; }
        public int TotalViewsCount { get; set; }
        public int TotalApplications { get; set; }
    }
}
