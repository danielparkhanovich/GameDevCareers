using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsContainerViewModelFactory : IFactory<CardsContainerViewModel>
    {
        private const string CardPartialViewName = "./JobOffers/_ApplicationCard";

        private readonly IRepository<OfferApplication> repository;
        private readonly CompanyPanelApplicationSearchParameters searchData;


        public CompanyApplicationsContainerViewModelFactory(IRepository<OfferApplication> repository,
            CompanyPanelApplicationSearchParameters searchData)
        {
            this.repository = repository;
            this.searchData = searchData;
        }

        public async Task<CardsContainerViewModel> Create()
        {
            var searcher = new OfferApplicationsSearcher(searchData);
            var applications = await searcher.Search(repository);

            var header = new CompanyApplicationsHeaderViewModelFactory()
                .CreateViewModel();

            var containerFactory = new CardsContainerViewModelFactory<CompanyApplicationCardViewModelFactory, OfferApplication>
                (applications, header, searchData, CardPartialViewName, searcher.AfterFiltersCount);

            var container = await containerFactory.Create();

            return container;
        }
    }
}
