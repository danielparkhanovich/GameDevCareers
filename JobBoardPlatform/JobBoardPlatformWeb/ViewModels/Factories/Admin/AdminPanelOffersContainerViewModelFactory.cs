using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminPanelOffersContainerViewModelFactory : IFactory<CardsContainerViewModel>
    {
        private const string CardPartialViewName = "./JobOffers/_JobOffer";

        private readonly IRepository<JobOffer> repository;
        private readonly CompanyPanelOfferSearchParameters searchParams;


        public AdminPanelOffersContainerViewModelFactory(IRepository<JobOffer> repository, CompanyPanelOfferSearchParameters searchParams)
        {
            this.repository = repository;
            this.searchParams = searchParams;
        }

        public async Task<CardsContainerViewModel> Create()
        {
            var searcher = new CompanyOffersSearcher(searchParams);
            var offers = await searcher.Search(repository);

            var header = new CompanyOfferHeaderViewModelFactory().CreateViewModel();

            var containerFactory = new CardsContainerViewModelFactory<CompanyOfferViewModelFactory, JobOffer>
                (offers, header, searchParams, CardPartialViewName, searcher.AfterFiltersCount);

            var container = await containerFactory.Create();

            return container;
        }
    }
}
