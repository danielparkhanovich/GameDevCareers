namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Users
{
    // TODO: rename -> Offer content display view model
    public class JobOfferDisplayViewModel
    {
        public string JobTitle { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string CompanyImageUrl { get; set; } = string.Empty;

        public string FullAddress { get; set; } = string.Empty;

        public string WorkLocationType { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string MainTechnologyType { get; set; } = string.Empty;

        // Move to other controller
        public string ContactForm { get; set; } = string.Empty;

        public string[] SalaryDetails { get; set; }

        public string[] EmploymentDetails { get; set; }

        public string[]? TechKeywords { get; set; }
    }
}
