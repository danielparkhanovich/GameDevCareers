using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobBoardPlatformWeb.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;
        private readonly IRepository<JobOffer> offersRepository;


        public HomeController(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await UpdateDisplayView();

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

        private async Task<JobOffersDisplayViewModel> UpdateDisplayView()
        {
            var offersSet = await offersRepository.GetAllSet();
            var offers = await offersSet.Where(offer => offer.IsPublished)
                .OrderByDescending(offer => offer.PublishedAt)
                .Include(offer => offer.CompanyProfile)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .Include(offer => offer.ContactDetails)
                .ThenInclude(details => details.ContactType)
                .ToListAsync();

            var display = new JobOffersDisplayViewModel();

            var offersDisplay = new List<JobOfferCardDisplayViewModel>();
            foreach (var offer in offers)
            {
                var displayCard = GetDisplayCard(offer);

                offersDisplay.Add(displayCard);
            }

            display.JobOffers = offersDisplay;

            return display;
        }

        private JobOfferCardDisplayViewModel GetDisplayCard(JobOffer offer)
        {
            string salaryDetails = "Undisclosed Salary";

            if (offer.JobOfferEmploymentDetails != null && offer.JobOfferEmploymentDetails.Count > 0)
            {
                var details = offer.JobOfferEmploymentDetails.Where(x => x.SalaryRange != null);

                if (!details.IsNullOrEmpty())
                {
                    var firstDetails = details.OrderBy(x => x.SalaryRange!.To).First();
                    var salary = firstDetails.SalaryRange!;
                    salaryDetails = $"{salary.From} - {salary.To} {salary.SalaryCurrency.Type}";
                }
            }

            string publishedAgo = $"0d ago";

            if (offer.IsPublished)
            {
                publishedAgo = $"{(DateTime.Now - offer.PublishedAt).Days}d ago";
            }

            var techKeyWords = offer.TechKeywords.Select(x => x.Name).ToArray();

            var displayCard = new JobOfferCardDisplayViewModel()
            {
                Id = offer.Id,
                Company = offer.CompanyProfile.CompanyName,
                CompanyImageUrl = offer.CompanyProfile.ProfileImageUrl,
                JobTitle = offer.JobTitle,
                Country = offer.Country,
                City = offer.City,
                WorkLocationType = offer.WorkLocation.Type,
                SalaryDetails = salaryDetails,
                PublishedAgo = publishedAgo,
                TechKeywords = techKeyWords,
            };

            return displayCard;
        }
    }
}