using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Offer.Company;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class CompanyOfferCardsViewModelFactory : IFactory<ContainerCardsViewModel>
    {
        private const string CardPartialViewModelName = "./JobOffers/_JobOfferCompanyView";

        private readonly IRepository<JobOffer> offersRepository;
        private readonly int profileId;

        private readonly int page;
        private readonly int pageSize;
        private readonly bool[] filterToggles;
        private readonly SortType sortType;
        private readonly SortCategoryType sortCategoryType;


        public CompanyOfferCardsViewModelFactory(int profileId,
            IRepository<JobOffer> offersRepository,
            int page,
            int pageSize,
            bool[] filterToggles,
            SortType sortType,
            SortCategoryType sortCategoryType)
        {
            this.profileId = profileId;
            this.offersRepository = offersRepository;

            this.page = page;
            this.pageSize = pageSize;
            this.filterToggles = filterToggles;
            this.sortType = sortType;
            this.sortCategoryType = sortCategoryType;
        }

        public async Task<ContainerCardsViewModel> Create()
        {
            var offersLoader = new LoadCompanyOffersPage(offersRepository, 
                profileId,
                page,
                pageSize,
                filterToggles,
                sortType,
                sortCategoryType);
            var offers = await offersLoader.Load();

            var offerCards = new List<IContainerCard>();
            foreach (var offer in offers)
            {
                var offerCard = await GetOfferCard(offer);

                offerCards.Add(offerCard);
            }

            var filterLabels = new string[]
            {
                "Published",
                "Shelved"
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
                RelatedId = profileId,
                ContainerCards = offerCards,
                Page = page,
                SortType = sortType,
                SortCategory = sortCategoryType,
                FilterLabels = filterLabels,
                CardPartialViewModelName = CardPartialViewModelName,
                SortLabels = sortLables,
                SortCategoryTypes = sortCategoryTypes,
                FilterToggles = filterToggles,
                RecordsCount = offersLoader.SelectedCount
            };

            return viewModel;
        }

        private async Task<CompanyOfferCardViewModel> GetOfferCard(JobOffer offer)
        {
            var offerCardFactory = new CompanyOfferViewModelFactory(offer);

            var offerCard = await offerCardFactory.Create();

            return offerCard;
        }
    }
}
