using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Search.Contracts;
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
        private readonly CompanyOffersSearcher searcher;
        private int totalRecordsCount;


        public AdminPanelOffersContainerViewModelFactory(CompanyOffersSearcher searcher)
        {
            this.searcher = searcher;
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var headerViewModelFactory = new CompanyOfferHeaderViewModelFactory();
            return headerViewModelFactory.CreateViewModel();
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searchResponse = await searcher.Search();
            totalRecordsCount = searchResponse.TotalRecordsAfterFilters;

            var cardFactory = new AdminOfferViewModelFactory();
            return GetCards(cardFactory, searchResponse.Entities);
        }

        protected override IPageSearchParams GetSearchParams()
        {
            return searcher.SearchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecordsCount;
        }
    }
}
