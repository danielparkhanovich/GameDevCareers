using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminPanelCompaniesContainerViewModelFactory : IFactory<CardsContainerViewModel>
    {
        private const string CardPartialViewName = "./JobOffers/_JobOffer";

        private readonly IRepository<CompanyProfile> profileRepository;
        private readonly IRepository<CompanyIdentity> identityRepository;
        private readonly CompanyPanelOfferSearchParameters searchParams;


        public AdminPanelCompaniesContainerViewModelFactory(IRepository<CompanyProfile> profileRepository,
            IRepository<CompanyIdentity> identityRepository,
            CompanyPanelOfferSearchParameters searchParams)
        {
            this.profileRepository = profileRepository;
            this.identityRepository = identityRepository;
            this.searchParams = searchParams;
        }

        public async Task<CardsContainerViewModel> Create()
        {
            /*var searcher = new CompanyOffersSearcher(searchParams);
            var companies = await searcher.Search(repository);

            var header = new CompanyOfferHeaderViewModelFactory().CreateViewModel();

            var containerFactory = new CardsContainerViewModelFactory<CompanyOfferViewModelFactory, JobOffer>
                (offers, header, searchParams, CardPartialViewName, searcher.AfterFiltersCount);

            var container = await containerFactory.Create();

            return container;*/
            return null;
        }
    }
}
