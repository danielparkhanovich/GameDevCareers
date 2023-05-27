using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminPanelOffersContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<JobOffer>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly CompanyPanelOfferSearchParameters searchParams;
        private int totalRecordsCount;


        public AdminPanelOffersContainerViewModelFactory(IRepository<JobOffer> repository, 
            CompanyPanelOfferSearchParameters searchParams)
        {
            this.repository = repository;
            this.searchParams = searchParams;
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var headerViewModelFactory = new CompanyOfferHeaderViewModelFactory();
            return headerViewModelFactory.CreateViewModel();
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searcher = new CompanyOffersSearcher(searchParams);

            var offers = await searcher.Search(repository);
            totalRecordsCount = searcher.AfterFiltersCount;

            var cardFactory = new AdminOfferViewModelFactory();

            return GetCards(cardFactory, offers);
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
