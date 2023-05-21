using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class OffersMainPageViewModelFactory : IFactory<OffersMainPageViewModel>
    {
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
            var searchParamsFactory = new MainPageOfferSearchParametersFactory(request);
            var searchParams = searchParamsFactory.Create();

            var viewModel = new OffersMainPageViewModel();

            var mainPageOfferCardsFactory = new MainPageContainerViewModelFactory(offersRepository,
                searchParams,
                distributedCache);
            viewModel.OffersContainer = await mainPageOfferCardsFactory.Create();
            viewModel.OfferSearchData = searchParams;

            return viewModel;
        }
    }
}
