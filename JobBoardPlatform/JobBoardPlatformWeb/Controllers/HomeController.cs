using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatformWeb.Controllers
{
    public class HomeController : CardsControllerBase
    {
        private readonly MainPageOffersSearcherCacheDecorator searcher;


        public HomeController(MainPageOffersSearcherCacheDecorator searcher)
        {
            this.searcher = searcher;
        }

        [HttpPost]
        [Route(RefreshCardsContainerAction)]
        public override async Task<IActionResult> RefreshCardsContainer()
        {
            var container = await GetContainer();
            return PartialView(CardsContainerViewModel.PartialView, container);
        }

        [Route("")]
        [Route("commissions", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(searcher, GetSearchParams());
            var model = await viewModelFactory.CreateAsync();
            return View(model);
        }

        protected override async Task<CardsContainerViewModel> GetContainer()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(searcher, GetSearchParams());
            var model = await viewModelFactory.CreateAsync();
            return model.OffersContainer;
        }

        private MainPageOfferSearchParams GetSearchParams()
        {
            var searchParamsFactory = new MainPageOfferSearchParamsFactory();
            return searchParamsFactory.GetSearchParams(Request);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("None");
        }

        public async Task<IActionResult> LogOut()
        {
            var sessionManager = new AuthorizationService(HttpContext);
            await sessionManager.SignOutHttpContextAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}