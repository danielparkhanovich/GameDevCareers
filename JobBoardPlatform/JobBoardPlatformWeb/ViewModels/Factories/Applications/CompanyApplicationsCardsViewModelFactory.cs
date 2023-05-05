using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsCardsViewModelFactory : IFactory<ContainerCardsViewModel>
    {
        private const string CardPartialViewModelName = "./JobOffers/_ApplicationCard";

        private readonly IRepository<OfferApplication> repository;
        private readonly int offerId;

        private readonly int page;
        private readonly int pageSize;
        private readonly bool[] filterToggles;
        private readonly SortType sortType;
        private readonly SortCategoryType sortCategoryType;


        public CompanyApplicationsCardsViewModelFactory(int offerId,
            IRepository<OfferApplication> repository,
            int page,
            int pageSize,
            bool[] filterToggles,
            SortType sortType,
            SortCategoryType sortCategoryType)
        {
            this.offerId = offerId;
            this.repository = repository;
            this.page = page;
            this.pageSize = pageSize;
            this.filterToggles = filterToggles;
            this.sortType = sortType;
            this.sortCategoryType = sortCategoryType;
        }

        public async Task<ContainerCardsViewModel> Create()
        {
            var applicationsLoader = new LoadApplicationsPage(repository,
                offerId,
                page,
                pageSize,
                filterToggles,
                sortType,
                sortCategoryType);
            var applications = await applicationsLoader.Load();

            var applicationCards = new List<IContainerCard>(applications.Count);
            foreach (var application in applications)
            {
                var applicationCardFactory = new CompanyApplicationCardViewModelFactory(application);
                var applicationCard = await applicationCardFactory.Create();
                applicationCards.Add(applicationCard);
            }

            var filterLabels = new string[] 
            { 
                "Unseen", 
                "<i class=\"bi bi-star\"></i>", 
                "<i class=\"bi bi-question-lg\"></i>", 
                "<i class=\"bi bi-x-lg\"></i>" 
            };

            var sortCategoryTypes = Enum.GetValues(typeof(SortCategoryType)).Cast<SortCategoryType>().ToArray();
            var sortLables = new string[] 
            { 
                "Date", 
                "Alphabetically", 
                "Relevenacy" 
            }; 

            var viewModel = new ContainerCardsViewModel()
            {
                RelatedId = offerId,
                ContainerCards = applicationCards,
                Page = page,
                SortType = sortType,
                SortCategory = sortCategoryType,
                FilterLabels = filterLabels,
                CardPartialViewModelName = CardPartialViewModelName,
                SortLabels = sortLables,
                SortCategoryTypes = sortCategoryTypes,
                FilterToggles = filterToggles,
                RecordsCount = applicationsLoader.SelectedApplicationsCount,
                PageSize = pageSize
            };

            return viewModel;
        }
    }
}
