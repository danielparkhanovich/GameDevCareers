using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.OfferViewModels.Company
{
    public class CompanyOfferCardViewModel : IContainerCard
    {
        public int TotalViews { get; set; }
        public int TotalApplicants { get; set; }
        public string MainTechnology { get; set; } = string.Empty;
        public string ContactType { get; set; } = string.Empty;
        public string? ContactAddress { get; set; }
        public bool IsPublished { get; set; }
        public bool IsShelved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPaid { get; set; }
        public bool IsVisible { get; set; }
        public bool IsAvailable { get; set; }
        public OfferStateType StateType { get; set; }
        public OfferCardViewModel CardDisplay { get; set; }
    }
}
