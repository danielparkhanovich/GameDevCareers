using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class MainPageContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<JobOffer>
    {
        private readonly MainPageOffersSearcherCacheDecorator offersSearcher;
        private readonly MainPageOfferSearchParams searchParams;
        private int totalRecordsAfterFilters;


        public MainPageContainerViewModelFactory(
            MainPageOffersSearcherCacheDecorator offersSearcher, MainPageOfferSearchParams searchParams)
        {
            this.offersSearcher = offersSearcher;
            this.searchParams = searchParams;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searchResponse = await offersSearcher.Search(searchParams);
            totalRecordsAfterFilters = searchResponse.TotalRecordsAfterFilters;

            var cardFactory = new OfferCardViewModelFactory();
            return GetCards(cardFactory, searchResponse.Entities);
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            return null; 
        }

        protected override IPageSearchParams GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecordsAfterFilters;
        }
    }
}
