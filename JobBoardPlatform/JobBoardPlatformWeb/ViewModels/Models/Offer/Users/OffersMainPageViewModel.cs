using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OffersMainPageViewModel : IMainTechnology
    {
        public int MainTechnologyType { get => OfferSearchData.MainTechnology; set => OfferSearchData.MainTechnology = value; }

        public CardsContainerViewModel OffersContainer { get; set; }
        public MainPageOfferSearchParameters OfferSearchData { get; set; }
    }
}
