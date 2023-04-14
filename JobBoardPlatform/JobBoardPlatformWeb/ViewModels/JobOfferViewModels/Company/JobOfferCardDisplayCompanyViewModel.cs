namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company
{
    public class JobOfferCardDisplayCompanyViewModel
    {
        public string MainTechnology { get; set; } = string.Empty;
        public string ContactType { get; set; } = string.Empty;
        public string? ContactAddress { get; set; }
        public bool IsPublished { get; set; }
        public JobOfferCardDisplayViewModel CardDisplay { get; set; }
    }
}
