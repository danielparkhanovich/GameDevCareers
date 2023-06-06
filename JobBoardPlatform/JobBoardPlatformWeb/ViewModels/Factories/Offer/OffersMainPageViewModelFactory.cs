using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IViewModelAsyncFactory<OffersMainPageViewModel>
    {
        private readonly MainPageOffersSearcherCacheDecorator offersSearcher;
        private readonly MainPageOfferSearchParams searchParams;


        public OffersMainPageViewModelFactory(
            MainPageOffersSearcherCacheDecorator offersSearcher, MainPageOfferSearchParams searchParams)
        {
            this.offersSearcher = offersSearcher;
            this.searchParams = searchParams;
        }

        public async Task<OffersMainPageViewModel> CreateAsync()
        {
            var viewModel = new OffersMainPageViewModel();
            var mainPageOfferCardsFactory = new MainPageContainerViewModelFactory(
                offersSearcher, searchParams);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.CreateAsync();
            viewModel.OfferSearchData = searchParams;
            return viewModel;
        }
    }
}
