using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Offer.Company;
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

            var viewModel = new CompanyApplicationsViewModel();
            viewModel.Applications = await GetApplicationCards();

            viewModel.OfferCard = await GetOfferCard(offer);

            viewModel.TotalApplications = offer.NumberOfApplications;
            viewModel.TotalViewsCount = offer.NumberOfViews;

            return viewModel;
        }

        private async Task<CompanyApplicationsCardsViewModel> GetApplicationCards()
        {
            var applicationCardsFactory = new CompanyApplicationsCardsViewModelFactory(offerId,
                applicationsRepository, 
                page,
                pageSize,
                filterStates, 
                SortType.Descending, 
                SortCategoryType.PublishDate);

            var applicationCards = await applicationCardsFactory.Create();

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
