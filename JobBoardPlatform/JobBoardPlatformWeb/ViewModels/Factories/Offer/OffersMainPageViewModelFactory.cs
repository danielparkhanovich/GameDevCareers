using JobBoardPlatform.BLL.Search.Offers;
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
        private readonly HttpRequest request;


        public OffersMainPageViewModelFactory(IRepository<JobOffer> offersRepository, HttpRequest request)
        {
            this.offersRepository = offersRepository;
            this.request = request;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            var viewModel = new OffersMainPageViewModel();
            var mainPageOfferCardsFactory = new MainPageContainerCardsViewModelFactory(offersRepository,
                request,
                PageSize);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.Create();

            var searchData = new OfferSearchDataUrlFactory(request).Create();
            viewModel.OfferSearchData = searchData;

            viewModel.OffersContainer.Page = searchData.Page;

            return viewModel;
        }
    }
}
