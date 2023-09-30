using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminOfferCardViewModel : IContainerCard
    {
        public string PartialView => "./Admin/_JobOfferAdminView";

        public int Id { get => CardViewModel.CardDisplay.Id; set => CardViewModel.CardDisplay.Id = value; }
        public CompanyOfferCardViewModel CardViewModel { get; set; } = new CompanyOfferCardViewModel();
        public bool IsSuspended { get; set; }
    }
}
