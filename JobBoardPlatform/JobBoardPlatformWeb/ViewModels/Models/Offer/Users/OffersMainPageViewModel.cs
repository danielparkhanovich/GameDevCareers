using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OffersMainPageViewModel : IMainTechnology
    {
        public ContainerCardsViewModel OffersContainer { get; set; }
        public int MainTechnologyType { get; set; }
    }
}
