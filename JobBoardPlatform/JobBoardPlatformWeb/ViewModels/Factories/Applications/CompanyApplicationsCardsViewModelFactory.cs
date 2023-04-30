using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Offer.Company;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsCardsViewModelFactory : IFactory<CompanyApplicationsCardsViewModel>
    {
        private readonly IRepository<OfferApplication> repository;
        private readonly int offerId;
        private readonly int page;
        private readonly int pageSize;
        private readonly bool[] filterStates;
        private readonly SortType sortType;
        private readonly SortCategoryType sortCategoryType;


        public CompanyApplicationsCardsViewModelFactory(int offerId,
            IRepository<OfferApplication> repository,
            int page,
            int pageSize,
            bool[] filterStates,
            SortType sortType,
            SortCategoryType sortCategoryType)
        {
            this.offerId = offerId;
            this.repository = repository;
            this.page = page;
            this.pageSize = pageSize;
            this.filterStates = filterStates;
            this.sortType = sortType;
            this.sortCategoryType = sortCategoryType;
        }

        public async Task<CompanyApplicationsCardsViewModel> Create()
        {
            var applicationsLoader = new LoadApplicationsPage(repository,
                offerId,
                page,
                pageSize,
                filterStates,
                sortType,
                sortCategoryType);
            var applications = await applicationsLoader.Load();

            var applicationCards = new List<CompanyApplicationCardViewModel>(applications.Count);
            foreach (var application in applications)
            {
                var applicationCardFactory = new CompanyApplicationCardViewModelFactory(application);
                var applicationCard = await applicationCardFactory.Create();
                applicationCards.Add(applicationCard);
            }

            var viewModel = new CompanyApplicationsCardsViewModel()
            {
                OfferId = offerId,
                Applications = applicationCards,
                Page = page,
                SortType = sortType,
                SortCategory = sortCategoryType,
                IsIncludeUnseen = filterStates[0],
                IsIncludeMustHire = filterStates[1],
                IsIncludeAverage = filterStates[2],
                IsIncludeReject = filterStates[3],
                RecordsCount = applicationsLoader.SelectedApplicationsCount
            };

            return viewModel;
        }
    }
}
