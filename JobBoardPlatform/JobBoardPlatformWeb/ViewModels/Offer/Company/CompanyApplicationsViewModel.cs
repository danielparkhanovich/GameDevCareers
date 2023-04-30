using JobBoardPlatform.PL.ViewModels.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Offer.Company.Contracts;
using JobBoardPlatform.PL.ViewModels.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.OfferViewModels.Company
{
    public class CompanyApplicationsViewModel
    {
        public OfferCardViewModel OfferCard { get; set; }
        public CompanyApplicationsCardsViewModel? Applications { get; set; }
        public int TotalViewsCount { get; set; }
        public int TotalApplications { get; set; }
    }
}
