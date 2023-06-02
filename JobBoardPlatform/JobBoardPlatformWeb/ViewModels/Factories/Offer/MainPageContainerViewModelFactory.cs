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
        private readonly IFilteringSearcher<JobOffer, MainPageOfferSearchParams> offersSearcher;
        private int totalRecordsAfterFilters;


        public MainPageContainerViewModelFactory(MainPageOffersSearcherCacheDecorator offersSearcher)
        {
            this.offersSearcher = offersSearcher;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searchResponse = await offersSearcher.Search();
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
            return offersSearcher.SearchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecordsAfterFilters;
        }
    }
}
