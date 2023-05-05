using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.MainPage
{
    public class MainPageContainerCardsViewModelFactory : IFactory<ContainerCardsViewModel>
    {
        private const string CardPartialViewModelName = "./JobOffers/_JobOffer";

        private readonly IRepository<JobOffer> repository;

        private readonly int page;
        private readonly int pageSize;
        private readonly bool[] filterToggles;
        private readonly SortType sortType;
        private readonly SortCategoryType sortCategoryType;


        public MainPageContainerCardsViewModelFactory(IRepository<JobOffer> repository,
            int page,
            int pageSize,
            bool[] filterToggles,
            SortType sortType,
            SortCategoryType sortCategoryType)
        {
            this.repository = repository;
            this.page = page;
            this.pageSize = pageSize;
            this.filterToggles = filterToggles;
            this.sortType = sortType;
            this.sortCategoryType = sortCategoryType;
        }

        public async Task<ContainerCardsViewModel> Create()
        {
            var offersLoader = new LoadActualOffersPage(repository, page, pageSize);
            var offers = await offersLoader.Load();

            var offerCards = new List<IContainerCard>(offers.Count);
            foreach (var offer in offers)
            {
                var offerCardFactory = new OfferCardViewModelFactory(offer);
                var offerCard = await offerCardFactory.Create();

                offerCards.Add(offerCard);
            }

            var filterLabels = new string[0];
            var sortCategoryTypes = new SortCategoryType[0];
            var sortLables = new string[0];

            var viewModel = new ContainerCardsViewModel()
            {
                RelatedId = 0,
                ContainerCards = offerCards,
                Page = page,
                SortType = sortType,
                SortCategory = sortCategoryType,
                FilterLabels = filterLabels,
                CardPartialViewModelName = CardPartialViewModelName,
                SortLabels = sortLables,
                SortCategoryTypes = sortCategoryTypes,
                FilterToggles = filterToggles,
                RecordsCount = offersLoader.SelectedOffersCount,
                PageSize = pageSize,
                IsShowHeader = false
            };

            return viewModel;
        }
    }
}
