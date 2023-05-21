using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace JobBoardPlatformWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IDistributedCache distributedCache;


        public HomeController(IRepository<JobOffer> offersRepository, IDistributedCache distributedCache)
        {
            this.offersRepository = offersRepository;
            this.distributedCache = distributedCache;
        }

        [Route("")]
        [Route("commissions", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, 
                Request, 
                distributedCache);
            var model = await viewModelFactory.Create();

            return View(model);
        }

        [Route("RefreshCardContainer")]
        [HttpPost]
        public async virtual Task<IActionResult> RefreshCardContainer()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, 
                Request, 
                distributedCache);
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