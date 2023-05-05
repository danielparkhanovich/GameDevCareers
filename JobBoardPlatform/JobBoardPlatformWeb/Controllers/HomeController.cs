using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
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

        public async Task<IActionResult> Index()
        {
            var viewModelFactory = new OffersMainPageViewModelFactory(offersRepository, 1);

            var model = await viewModelFactory.Create();

            return View(model);
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