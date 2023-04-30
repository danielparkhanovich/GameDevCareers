using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;

namespace JobBoardPlatform.PL.ViewModels.Offer.Company
{
    public class CompanyApplicationsCardsViewModel
    {
        public int OfferId { get; set; }
        public ICollection<CompanyApplicationCardViewModel>? Applications { get; set; }
        public int Page { get; set; }
        public SortCategoryType SortCategory { get; set; }
        public SortType SortType { get; set; }
        public bool IsIncludeUnseen { get; set; }
        public bool IsIncludeMustHire { get; set; }
        public bool IsIncludeAverage { get; set; }
        public bool IsIncludeReject { get; set; }
        public int RecordsCount { get; set; }
    }
}
