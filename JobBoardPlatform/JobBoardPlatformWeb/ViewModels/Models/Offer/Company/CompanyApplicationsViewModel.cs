using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class CompanyApplicationsViewModel
    {
        public OfferCardViewModel OfferCard { get; set; }
        public CardsContainerViewModel? Applications { get; set; }
        public int TotalViewsCount { get; set; }
        public int TotalApplications { get; set; }
    }
}
