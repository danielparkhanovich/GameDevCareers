using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class MainPageContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<JobOffer>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;
        private readonly MainPageOfferSearchParameters searchParams;
        private int totalRecordsCount;


        public MainPageContainerViewModelFactory(IRepository<JobOffer> repository, 
            MainPageOfferSearchParameters searchParams,
            ICacheRepository<List<JobOffer>> offersCache,
            ICacheRepository<int> offersCountCache)
        {
            this.repository = repository;
            this.searchParams = searchParams;
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searcher = new SearchActualOffersCache(searchParams, offersCache, offersCountCache);
            var offers = await searcher.Search(repository);
            totalRecordsCount = searcher.AfterFiltersCount;

            var cardFactory = new OfferCardViewModelFactory();
            return GetCards(cardFactory, offers);
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            return null; 
        }

        protected override ISearchParameters GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecordsCount;
        }
    }
}
