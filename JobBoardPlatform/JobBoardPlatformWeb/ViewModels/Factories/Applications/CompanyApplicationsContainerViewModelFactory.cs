using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.BLL.Search.CompanyPanel.Applications;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<JobOfferApplication>
    {
        private readonly OfferApplicationsSearcher searcher;
        private readonly CompanyPanelApplicationSearchParams searchParams;
        private int totalResult;


        public CompanyApplicationsContainerViewModelFactory(
            OfferApplicationsSearcher searcher, CompanyPanelApplicationSearchParams searchParams)
        {
            this.searcher = searcher;
            this.searchParams = searchParams;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searchResponse = await searcher.Search(searchParams);
            totalResult = searchResponse.TotalRecordsAfterFilters;

            var cardFactory = new CompanyApplicationCardViewModelFactory();
            return GetCards(cardFactory, searchResponse.Entities);
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var header = new CompanyApplicationsHeaderViewModelFactory();
            return header.CreateViewModel();
        }

        protected override IPageSearchParams GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalResult;
        }
    }
}
