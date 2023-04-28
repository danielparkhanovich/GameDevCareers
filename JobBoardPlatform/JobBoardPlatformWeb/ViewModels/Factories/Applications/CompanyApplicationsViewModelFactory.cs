using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsViewModelFactory : IFactory<CompanyApplicationsViewModel>
    {
        private readonly int offerId;
        private readonly int page;
        private readonly int pageSize;
        private readonly bool[] filterStates;
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IRepository<JobOffer> offersRepository;
        

        public CompanyApplicationsViewModelFactory(int offerId, 
            int page, 
            int pageSize,
            bool[] filterStates,
            IRepository<OfferApplication> applicationsRepository,
            IRepository<JobOffer> offersRepository)
        {
            this.offerId = offerId;
            this.page = page;
            this.pageSize = pageSize;
            this.filterStates = filterStates;
            this.applicationsRepository = applicationsRepository;
            this.offersRepository = offersRepository;
        }

        public async Task<CompanyApplicationsViewModel> Create()
        {
            var offerLoader = new LoadCompanyOffer(offersRepository, offerId);
            var offer = await offerLoader.Load();

            var applicationsLoader = new LoadApplicationsPage(applicationsRepository,
                    offerId,
                    page,
                    pageSize,
                    filterStates);
            var applications = await applicationsLoader.Load();

            var viewModel = new CompanyApplicationsViewModel();
            viewModel.Applications = await GetApplicationCards(applications);
            viewModel.OfferCard = await GetOfferCard(offer);

            viewModel.TotalApplications = offer.NumberOfApplications;
            viewModel.TotalViewsCount = offer.NumberOfViews;
            viewModel.AfterFiltersApplications = applicationsLoader.SelectedApplicationsCount;

            viewModel.Page = page;

            viewModel.SortBy = string.Empty;
            viewModel.IsIncludeUnseen = filterStates[0];
            viewModel.IsIncludeMustHire = filterStates[1];
            viewModel.IsIncludeAverage = filterStates[2];
            viewModel.IsIncludeReject = filterStates[3];

            return viewModel;
        }

        private async Task<List<CompanyApplicationCardViewModel>> GetApplicationCards(List<OfferApplication> applications)
        {
            var applicationCards = new List<CompanyApplicationCardViewModel>(applications.Count);
            foreach (var application in applications)
            {
                var applicationCardFactory = new CompanyApplicationCardViewModelFactory(application);
                var applicationCard = await applicationCardFactory.Create();
                applicationCards.Add(applicationCard);
            }
            return applicationCards;
        }

        private async Task<OfferCardViewModel> GetOfferCard(JobOffer offer)
        {
            var offerCardFactory = new OfferCardViewModelFactory(offer);
            var offerCard = await offerCardFactory.Create();
            return offerCard;
        }
    }
}
