using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.MainPage;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
        private const int PageSize = 20;

        private readonly IRepository<JobOffer> offersRepository;
        private readonly int page;


        public OffersMainPageViewModelFactory(IRepository<JobOffer> offersRepository, int page)
        {
            this.offersRepository = offersRepository;
            this.page = page;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            bool[] filterToggles = new bool[0];

            var viewModel = new OffersMainPageViewModel();
            var mainPageOfferCardsFactory = new MainPageContainerCardsViewModelFactory(offersRepository,
                page, 
                PageSize, 
                filterToggles, 
                SortType.Descending, 
                SortCategoryType.PublishDate);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.Create();
            viewModel.MainTechnologyType = 0;

            return viewModel;
        }
    }
}
