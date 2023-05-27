using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<OfferApplication>
    {
        private readonly IRepository<OfferApplication> repository;
        private readonly CompanyPanelApplicationSearchParameters searchParams;
        private int totalResult;


        public CompanyApplicationsContainerViewModelFactory(IRepository<OfferApplication> repository,
            CompanyPanelApplicationSearchParameters searchParams)
        {
            this.repository = repository;
            this.searchParams = searchParams;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searcher = new OfferApplicationsSearcher(searchParams);
            totalResult = searcher.AfterFiltersCount;

            var applications = await searcher.Search(repository);
            var cardFactory = new CompanyApplicationCardViewModelFactory();
            return GetCards(cardFactory, applications);
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var header = new CompanyApplicationsHeaderViewModelFactory();
            return header.CreateViewModel();
        }

        protected override ISearchParameters GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalResult;
        }
    }
}
