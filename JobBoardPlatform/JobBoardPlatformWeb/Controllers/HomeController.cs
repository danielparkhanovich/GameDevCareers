using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatformWeb.Controllers
{
    [Route("")]
    public class HomeController : CardsControllerBase
    {
        private readonly MainPageOffersSearcherCacheDecorator searcher;


        public HomeController(MainPageOffersSearcherCacheDecorator searcher)
        {
            this.searcher = searcher;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(searcher, GetSearchParams());
            var model = await viewModelFactory.CreateAsync();
            return View(model);
        }

        [Route("terms-of-service")]
        public IActionResult TermsOfService()
        {
            return View();
        }

        [Route("privacy-policy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
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

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("None");
        }

        [Route("log-out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}