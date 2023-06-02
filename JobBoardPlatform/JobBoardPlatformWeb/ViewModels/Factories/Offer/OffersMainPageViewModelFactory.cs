using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
        private readonly MainPageOffersSearcherCacheDecorator offersSearcher;


        public OffersMainPageViewModelFactory(MainPageOffersSearcherCacheDecorator offersSearcher)
        {
            this.offersSearcher = offersSearcher;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            var viewModel = new OffersMainPageViewModel();
            var mainPageOfferCardsFactory = new MainPageContainerViewModelFactory(offersSearcher);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.Create();
            viewModel.OfferSearchData = offersSearcher.SearchParams;
            return viewModel;
        }
    }
}
