using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class CompanyOffersContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<JobOffer>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly CompanyPanelOfferSearchParameters searchParams;
        private int totalResults;


        public CompanyOffersContainerViewModelFactory(IRepository<JobOffer> repository,
            CompanyPanelOfferSearchParameters searchParams)
        {
            this.repository = repository;
            this.searchParams = searchParams;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searcher = new CompanyOffersSearcher(searchParams);
            totalResults = searcher.AfterFiltersCount;

            var offers = await searcher.Search(repository);
            var cardFactory = new CompanyOfferViewModelFactory();
            return GetCards(cardFactory, offers);
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var header = new CompanyOfferHeaderViewModelFactory();
            return header.CreateViewModel();
        }

        protected override ISearchParameters GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalResults;
        }
    }
}
