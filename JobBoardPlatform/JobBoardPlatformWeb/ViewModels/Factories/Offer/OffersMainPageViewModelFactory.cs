using JobBoardPlatform.BLL.Search.Offers;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.MainPage;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
        private const int PageSize = 20;

        private readonly IRepository<JobOffer> offersRepository;
        private readonly HttpRequest request;
        private readonly IDistributedCache distributedCache;


        public OffersMainPageViewModelFactory(IRepository<JobOffer> offersRepository, 
            HttpRequest request,
            IDistributedCache distributedCache)
        {
            this.offersRepository = offersRepository;
            this.request = request;
            this.distributedCache = distributedCache;
        }

        public async Task<OffersMainPageViewModel> Create()
        {
            var viewModel = new OffersMainPageViewModel();

            var searchData = new OfferSearchDataUrlFactory(request).Create();
            viewModel.OfferSearchData = searchData;

            var mainPageOfferCardsFactory = new MainPageContainerCardsViewModelFactory(offersRepository,
                searchData,
                distributedCache,
                PageSize);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.Create();

            viewModel.OffersContainer.Page = searchData.Page;

            return viewModel;
        }
    }
}
