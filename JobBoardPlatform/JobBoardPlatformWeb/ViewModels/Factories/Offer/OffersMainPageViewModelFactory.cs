using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
        private readonly MainPageOffersSearcherCacheDecorator offersSearcher;
        private readonly MainPageOfferSearchParams searchParams;


        public OffersMainPageViewModelFactory(
            MainPageOffersSearcherCacheDecorator offersSearcher, MainPageOfferSearchParams searchParams)
        {
            this.offersSearcher = offersSearcher;
            this.searchParams = searchParams;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            var viewModel = new OffersMainPageViewModel();
            var mainPageOfferCardsFactory = new MainPageContainerViewModelFactory(
                offersSearcher, searchParams);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.Create();
            viewModel.OfferSearchData = searchParams;
            return viewModel;
        }
    }
}
