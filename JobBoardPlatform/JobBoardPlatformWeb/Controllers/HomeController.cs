using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatformWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;


        public HomeController(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        [Route("")]
        [Route("commissions", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, Request);

            var model = await viewModelFactory.Create();

            return View(model);
        }

        [HttpPost]
        public async virtual Task<IActionResult> RefreshCardContainer(ContainerCardsViewModel cardsViewModel)
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, Request);

            var model = await viewModelFactory.Create();
            cardsViewModel = model.OffersContainer;

            return PartialView("./Templates/_CardsContainer", cardsViewModel);
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