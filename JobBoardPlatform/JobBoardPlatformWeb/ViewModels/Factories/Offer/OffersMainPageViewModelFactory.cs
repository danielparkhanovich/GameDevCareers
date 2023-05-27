using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly HttpRequest request;
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;


        public OffersMainPageViewModelFactory(IRepository<JobOffer> offersRepository, 
            HttpRequest request,
            ICacheRepository<List<JobOffer>> offersCache,
            ICacheRepository<int> offersCountCache)
        {
            this.offersRepository = offersRepository;
            this.request = request;
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            var searchParamsFactory = new MainPageOfferSearchParametersFactory(request);
            var searchParams = searchParamsFactory.Create();

            var viewModel = new OffersMainPageViewModel();

            var mainPageOfferCardsFactory = new MainPageContainerViewModelFactory(offersRepository,
                searchParams,
                offersCache, 
                offersCountCache);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.Create();
            viewModel.OfferSearchData = searchParams;

            return viewModel;
        }
    }
}
