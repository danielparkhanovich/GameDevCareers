using JobBoardPlatform.BLL.Search.Offers;
using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminPanelCardsViewModelFactory : IFactory<ContainerCardsViewModel>
    {
        private const string CardPartialViewModelName = "./JobOffers/_JobOffer";

        private readonly IRepository<JobOffer> repository;
        private readonly OfferSearchData searchData;
        private readonly int pageSize;
        private readonly bool[] filterToggles;
        private readonly int page;


        public AdminPanelCardsViewModelFactory(IRepository<JobOffer> repository,
            OfferSearchData searchData,
            int pageSize,
            bool[] filterToggles,
            int page)
        {
            this.repository = repository;
            this.searchData = searchData;
            this.pageSize = pageSize;
            this.filterToggles = filterToggles;
            this.page = page;
        }

        public async Task<ContainerCardsViewModel> Create()
        {
            var offersSearcher = new SearchActualOffers(repository, searchData, pageSize);

            var offers = await offersSearcher.Search();

            var offerCards = new List<IContainerCard>(offers.Count);
            foreach (var offer in offers)
            {
                var offerCardFactory = new OfferCardViewModelFactory(offer);
                var offerCard = await offerCardFactory.Create();

                offerCards.Add(offerCard);
            }

            var filterLabels = new string[]
            {
                "Published",
                "Shelved"
            };

            var sortCategoryTypes = Enum.GetValues(typeof(SortCategoryType)).Cast<SortCategoryType>().ToArray();

            var sortLabels = new string[]
            {
                "Date",
                "Alphabetically",
                "Relevenacy"
            };

            var viewModel = new ContainerCardsViewModel()
            {
                RelatedId = 0, // excessive
                Page = page, // excessive
                SortType = SortType.Ascending, // excessive
                SortCategory = SortCategoryType.PublishDate, // excessive
                FilterToggles = filterToggles, // excessive 
                SortCategoryTypes = sortCategoryTypes, // excessive
                FilterLabels = filterLabels, // excessive -> eventually move into separate header
                SortLabels = sortLabels, // excessive -> eventually move into separate header
                CardPartialViewModelName = CardPartialViewModelName,
                ContainerCards = offerCards,
                RecordsCount = offersSearcher.AfterFiltersCount,
                PageSize = pageSize,
                IsShowHeader = true
            };

            return viewModel;
        }
    }
}
