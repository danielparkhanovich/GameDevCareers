using JobBoardPlatform.PL.ViewModels.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.OfferViewModels.Company
{
    public class CompanyApplicationsViewModel
    {
        public OfferCardViewModel OfferCard { get; set; }
        public ICollection<CompanyApplicationCardViewModel>? Applications { get; set; }

        public int TotalViewsCount { get; set; }
        public int TotalApplications { get; set; }
        public int AfterFiltersApplications { get; set; }
        public int Page { get; set; }
        public bool IsIncludeUnseen { get; set; }
        public bool IsIncludeMustHire { get; set; }
        public bool IsIncludeAverage { get; set; }
        public bool IsIncludeReject { get; set; }
    }
}
