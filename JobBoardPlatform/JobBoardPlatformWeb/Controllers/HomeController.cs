using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatformWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;


        public HomeController(IRepository<JobOffer> offersRepository, ICacheRepository<List<JobOffer>> offersCache,
            ICacheRepository<int> offersCountCache)
        {
            this.offersRepository = offersRepository;
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        [Route("")]
        [Route("commissions", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, 
                Request,
                offersCache,
                offersCountCache);
            var model = await viewModelFactory.Create();
            return View(model);
        }

        [Route("RefreshCardsContainer")]
        [HttpPost]
        public async virtual Task<IActionResult> RefreshCardsContainer()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, 
                Request, 
                offersCache,
                offersCountCache);
            var model = await viewModelFactory.Create();
            var container = model.OffersContainer;
            return PartialView("./Templates/_CardsContainer", container);
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