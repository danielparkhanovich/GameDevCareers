using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsViewModelFactory : IFactory<CompanyApplicationsViewModel>
    {
        private readonly CompanyPanelApplicationSearchParameters searchData;
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IRepository<JobOffer> offersRepository;
        

        public CompanyApplicationsViewModelFactory(CompanyPanelApplicationSearchParameters searchData,
            IRepository<OfferApplication> applicationsRepository,
            IRepository<JobOffer> offersRepository)
        {
            this.searchData = searchData;
            this.applicationsRepository = applicationsRepository;
            this.offersRepository = offersRepository;
        }

        public async Task<CompanyApplicationsViewModel> Create()
        {
            var offerLoader = new LoadCompanyOffer(offersRepository, searchData.OfferId);
            var offer = await offerLoader.Load();

            var viewModel = new CompanyApplicationsViewModel();
            viewModel.Applications = await GetApplicationCards();

            viewModel.OfferCard = GetOfferCard(offer);

            viewModel.TotalApplications = offer.NumberOfApplications;
            viewModel.TotalViewsCount = offer.NumberOfViews;

            return viewModel;
        }

        private async Task<CardsContainerViewModel> GetApplicationCards()
        {
            var applicationCardsFactory = new CompanyApplicationsContainerViewModelFactory(applicationsRepository, searchData);
            var applicationCards = await applicationCardsFactory.Create();

            return applicationCards;
        }

        private OfferCardViewModel GetOfferCard(JobOffer offer)
        {
            var offerCardFactory = new OfferCardViewModelFactory();
            var offerCard = offerCardFactory.CreateViewModel(offer);
            return (offerCard as OfferCardViewModel)!;
        }
    }
}
