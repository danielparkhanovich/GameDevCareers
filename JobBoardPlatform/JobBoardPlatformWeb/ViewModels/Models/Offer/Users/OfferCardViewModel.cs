using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OfferCardViewModel : IContainerCard
    {
        public string PartialView => "./JobOffers/_JobOffer";

        // For redirections only
        public int Id { get; set; }
        public string OnClickUrlDisplayCompanyName { get; set; }
        public string OnClickUrlDisplayOfferTitleName { get; set; }
        public string OnClickRedirectController { get; set; }
        public string OnClickRedirectAction { get; set; }
        

        // Display data
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string CompanyImageUrl { get; set; } = string.Empty;
        public string PublishedAgo { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string WorkLocationType { get; set; } = string.Empty;
        public string WorkLocationTypeText { get; set; } = string.Empty;
        public string SalaryDetails { get; set; } = string.Empty;
        public string[]? TechKeywords { get; set; }
    }
}
