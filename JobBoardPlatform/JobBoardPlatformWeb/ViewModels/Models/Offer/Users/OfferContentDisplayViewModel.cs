namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OfferContentDisplayViewModel
    {
        public string ContactForm { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyImageUrl { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;
        public string WorkLocationType { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public string InformationClause { get; set; } = string.Empty;
        public string? ProcessingDataInFutureClause { get; set; }
        public string? CustomConsentClauseTitle { get; set; }
        public string? CustomConsentClause { get; set; }
        public string MainTechnologyType { get; set; } = string.Empty;
        public string[]? SalaryDetails { get; set; }
        public string[]? EmploymentDetails { get; set; }
        public string[]? TechKeywords { get; set; }
        public string? ResumeUrl { get; set; }
    }
}
