using JobBoardPlatform.BLL.Search.Offers;
using JobBoardPlatform.DAL.Data.Enums.Sort;
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
        private const int PageSize = 20;

        private readonly IRepository<JobOffer> repository;
        private readonly HttpRequest request;


        public MainPageContainerCardsViewModelFactory(IRepository<JobOffer> repository, 
            HttpRequest request)
        {
            this.repository = repository;
            this.request = request;
        }

        public async Task<ContainerCardsViewModel> Create()
        {
            var offersSearcher = new SearchActualOffers(repository, request, PageSize);
            var offers = await offersSearcher.Search();

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

            var searchData = new OfferSearchDataUrlFactory(request).Create();

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
