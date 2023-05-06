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


        private readonly IRepository<JobOffer> repository;
        private readonly HttpRequest request;
        private readonly int pageSize;


        public MainPageContainerCardsViewModelFactory(IRepository<JobOffer> repository, 
            HttpRequest request,
            int pageSize)
        {
            this.repository = repository;
            this.request = request;
            this.pageSize = pageSize;
        }

        public async Task<ContainerCardsViewModel> Create()
        {
            var offersSearcher = new SearchActualOffers(repository, request, pageSize);
            var offers = await offersSearcher.Search();

            var offerCards = new List<IContainerCard>(offers.Count);
            foreach (var offer in offers)
            {
                var offerCardFactory = new OfferCardViewModelFactory(offer);
                var offerCard = await offerCardFactory.Create();

                offerCards.Add(offerCard);
            }

            var viewModel = new ContainerCardsViewModel()
            {
                RelatedId = 0, // excessive
                Page = 0, // excessive
                SortType = SortType.Ascending, // excessive
                SortCategory = SortCategoryType.PublishDate, // excessive
                FilterToggles = new bool[0], // excessive 
                SortCategoryTypes = new SortCategoryType[0], // excessive
                FilterLabels = new string[0], // excessive -> eventually move into separate header
                SortLabels = new string[0], // excessive -> eventually move into separate header
                CardPartialViewModelName = CardPartialViewModelName,
                ContainerCards = offerCards,
                RecordsCount = offersSearcher.AfterFiltersCount,
                PageSize = pageSize,
                IsShowHeader = false
            };

            return viewModel;
        }
    }
}
