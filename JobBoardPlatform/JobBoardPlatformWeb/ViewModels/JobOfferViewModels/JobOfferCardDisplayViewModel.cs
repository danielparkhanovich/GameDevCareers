namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels
{
    public class JobOfferCardDisplayViewModel
    {
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string CompanyImageUrl { get; set; } = string.Empty;
        public string PublishedAgo { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string WorkLocationType { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        public string SalaryCurrency { get; set; } = string.Empty;
        public string[]? TechKeyWords { get; set; }
    }
}
