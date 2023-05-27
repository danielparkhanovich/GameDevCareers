using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class CompanyOfferCardViewModel : IContainerCard
    {
        public string PartialView => "./JobOffers/_JobOfferCompanyView";

        public int Id { get => CardDisplay.Id; set => CardDisplay.Id = value; }

        public int TotalViews { get; set; }
        public int TotalApplicants { get; set; }
        public string DaysLeft { get; set; } = string.Empty;
        public string MainTechnology { get; set; } = string.Empty;
        public string ContactType { get; set; } = string.Empty;
        public string? ContactAddress { get; set; }
        public bool IsPublished { get; set; }
        public bool IsShelved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPaid { get; set; }
        public bool IsVisibleOnMainPage { get; set; }
        public bool IsAvailableForEdit { get; set; }
        public OfferStateType StateType { get; set; }
        public IContainerCard CardDisplay { get; set; }
    }
}
