using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminPanelOffersContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<JobOffer>
    {
        private readonly CompanyOffersSearcher searcher;
        private readonly CompanyPanelOfferSearchParameters searchParams;
        private int totalRecordsCount;


        public AdminPanelOffersContainerViewModelFactory(
            CompanyOffersSearcher searcher, CompanyPanelOfferSearchParameters searchParams)
        {
            this.searcher = searcher;
            this.searchParams = searchParams;
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var headerViewModelFactory = new CompanyOfferHeaderViewModelFactory();
            return headerViewModelFactory.CreateViewModel();
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searchResponse = await searcher.Search(searchParams);
            totalRecordsCount = searchResponse.TotalRecordsAfterFilters;

            var cardFactory = new AdminOfferViewModelFactory();
            return GetCards(cardFactory, searchResponse.Entities);
        }

        protected override IPageSearchParams GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecordsCount;
        }
    }
}
