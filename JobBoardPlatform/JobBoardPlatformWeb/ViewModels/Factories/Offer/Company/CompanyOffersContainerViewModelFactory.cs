using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class CompanyOffersContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<JobOffer>
    {
        private readonly CompanyOffersSearcher searcher;
        private readonly CompanyPanelOfferSearchParameters searchParams;
        private int totalRecords;


        public CompanyOffersContainerViewModelFactory(
            CompanyOffersSearcher searcher, CompanyPanelOfferSearchParameters searchParams)
        {
            this.searcher = searcher;
            this.searchParams = searchParams;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var searchResponse = await searcher.Search(searchParams);
            totalRecords = searchResponse.TotalRecordsAfterFilters;

            var cardFactory = new CompanyOfferViewModelFactory();
            return GetCards(cardFactory, searchResponse.Entities);
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var header = new CompanyOfferHeaderViewModelFactory();
            return header.CreateViewModel();
        }

        protected override IPageSearchParams GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecords;
        }
    }
}
