using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        [Route("commissions")]
        public async Task<IActionResult> Index()
        {
            string tab = Request.Path.ToString().ToLower();

            // Get all query parameters
            bool isSalary = Request.Query.ContainsKey("salary");
            bool isRemote = Request.Query.ContainsKey("remote");


            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, 1);

            var model = await viewModelFactory.Create();

            return View(model);
        }

        [HttpPost]
        public async virtual Task<IActionResult> RefreshCardContainer(ContainerCardsViewModel cardsViewModel)
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, cardsViewModel.Page);

            var model = await viewModelFactory.Create();

            return PartialView("./Templates/_CardsContainer", model);
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