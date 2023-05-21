using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class CompanyOffersContainerViewModelFactory : IFactory<CardsContainerViewModel>
    {
        private const string CardPartialViewName = "./JobOffers/_JobOfferCompanyView";

        private readonly IRepository<JobOffer> repository;
        private readonly CompanyPanelOfferSearchParameters searchData;


        public CompanyOffersContainerViewModelFactory(IRepository<JobOffer> repository,
            CompanyPanelOfferSearchParameters searchData)
        {
            this.repository = repository;
            this.searchData = searchData;
        }

        public async Task<CardsContainerViewModel> Create()
        {            
            var searcher = new CompanyOffersSearcher(searchData);
            var offers = await searcher.Search(repository);

            var header = new CompanyOfferHeaderViewModelFactory()
                .CreateViewModel();

            var containerFactory = new CardsContainerViewModelFactory<CompanyOfferViewModelFactory, JobOffer>
                (offers, header, searchData, CardPartialViewName, searcher.AfterFiltersCount);

            var container = await containerFactory.Create();

            return container;
        }
    }
}
