using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class MainPageContainerViewModelFactory : IFactory<CardsContainerViewModel>
    {
        private const string CardPartialViewName = "./JobOffers/_JobOffer";

        private readonly IRepository<JobOffer> repository;
        private readonly IDistributedCache distributedCache;
        private readonly MainPageOfferSearchParameters searchParams;


        public MainPageContainerViewModelFactory(IRepository<JobOffer> repository, 
            MainPageOfferSearchParameters searchParams, 
            IDistributedCache distributedCache)
        {
            this.repository = repository;
            this.searchParams = searchParams;
            this.distributedCache = distributedCache;
        }

        public async Task<CardsContainerViewModel> Create()
        {
            var searcher = new SearchActualOffersCache(searchParams, distributedCache);
            var offers = await searcher.Search(repository);

            var containerFactory = new CardsContainerViewModelFactory<OfferCardViewModelFactory, JobOffer>
                (offers, null, searchParams, CardPartialViewName, searcher.AfterFiltersCount);

            var container = await containerFactory.Create();

            return container;
        }
    }
}
