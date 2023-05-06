using JobBoardPlatform.BLL.Search.Offers;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OffersMainPageViewModel : IMainTechnology
    {
        public int MainTechnologyType { get => OfferSearchData.MainTechnology; set => OfferSearchData.MainTechnology = value; }

        public ContainerCardsViewModel OffersContainer { get; set; }
        public OfferSearchData OfferSearchData { get; set; }
    }
}
