namespace JobBoardPlatform.PL.ViewModels.Offer.Users
{
    public class OfferCardViewModel
    {
        // For redirections only
        public int Id { get; set; }

        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string CompanyImageUrl { get; set; } = string.Empty;
        public string PublishedAgo { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string WorkLocationType { get; set; } = string.Empty;
        public string SalaryDetails { get; set; } = string.Empty;
        public string[]? TechKeywords { get; set; }
    }
}
