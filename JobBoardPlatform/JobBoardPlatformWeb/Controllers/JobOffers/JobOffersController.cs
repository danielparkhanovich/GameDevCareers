using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.JobOffers
{
    // TODO: split into two or more controllers
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class JobOffersController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<CompanyProfile> profilesRepository;
        private readonly IMapper<JobOfferUpdateViewModel, JobOffer> viewModelToOffer;


        public JobOffersController(IRepository<JobOffer> offersRepository, IRepository<CompanyProfile> profilesRepository)
        {
            this.offersRepository = offersRepository;
            this.profilesRepository = profilesRepository;
            this.viewModelToOffer = new JobOfferViewModelToJobOfferMapper();
        }

        public async virtual Task<IActionResult> Offers()
        {
            var model = await UpdateDisplayView();

            return View(model);
        }

        public async virtual Task<IActionResult> AddOffer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> AddOffer(JobOfferUpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

                var offer = new JobOffer();
                offer.CompanyProfileId = profileId;
                offer.CreatedAt = DateTime.Now;

                viewModelToOffer.Map(viewModel, offer);

                await offersRepository.Add(offer);

                return RedirectToAction("Offers");
            }

            return View(viewModel);
        }

        private async Task<CompanyJobOffersDisplayViewModel> UpdateDisplayView()
        {
            int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
            var profile = await profilesRepository.Get(profileId);

            var offersSet = await offersRepository.GetAllSet();
            var offers = await offersSet.Where(offer => offer.CompanyProfileId == profileId)
                .OrderByDescending(offer => offer.CreatedAt)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeyWords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .ToListAsync();

            var display = new CompanyJobOffersDisplayViewModel();

            var offersDisplay = new List<JobOfferCardDisplayViewModel>();
            foreach (var offer in offers)
            {
                var displayCard = GetDisplayCard(offer, profile);
                offersDisplay.Add(displayCard);
            }

            display.JobOffers = offersDisplay;

            return display;
        }

        private JobOfferCardDisplayViewModel GetDisplayCard(JobOffer offer, CompanyProfile profile)
        {
            string salaryDetails = "Undisclosed Salary";

            if (offer.JobOfferEmploymentDetails != null && offer.JobOfferEmploymentDetails.Count > 0)
            {
                var firstDetails = offer.JobOfferEmploymentDetails.First();

                var salary = firstDetails.SalaryRange;
                if (salary != null)
                {
                    salaryDetails = $"{salary.From} - {salary.To} {salary.SalaryCurrency.Type}";
                }
            }

            string publishedAgo = $"0d ago";

            if (offer.IsPublished)
            {
                publishedAgo = $"{(DateTime.Now - offer.PublishedAt).Days}d ago";
            }

            var techKeyWords = offer.TechKeyWords.Select(x => x.Name).ToArray();

            var displayCard = new JobOfferCardDisplayViewModel()
            {
                Company = profile.CompanyName,
                CompanyImageUrl = profile.ProfileImageUrl,
                JobTitle = offer.JobTitle,
                Country = offer.Country,
                City = offer.City,
                WorkLocationType = offer.WorkLocation.Type,
                SalaryDetails = salaryDetails,
                PublishedAgo = publishedAgo,
                TechKeyWords = techKeyWords
            };

            return displayCard;
        }
    }
}
