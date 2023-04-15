using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Users;
using JobBoardPlatform.PL.ViewModels.Utilities;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.JobOffers
{
    // TODO: split into two or more controllers
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyJobOffersController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<CompanyProfile> profilesRepository;
        private readonly IMapper<JobOfferUpdateViewModel, JobOffer> viewModelToOffer;


        public CompanyJobOffersController(IRepository<JobOffer> offersRepository, 
            IRepository<CompanyProfile> profilesRepository, IRepository<TechKeyword> keyWordsRepository)
        {
            this.offersRepository = offersRepository;
            this.profilesRepository = profilesRepository;
            this.viewModelToOffer = new JobOfferViewModelToJobOfferMapper(keyWordsRepository);
        }

        public async virtual Task<IActionResult> CompanyOffers()
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

        private async Task<JobOffersDisplayCompanyViewModel> UpdateDisplayView()
        {
            int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
            var profile = await profilesRepository.Get(profileId);

            var offersSet = await offersRepository.GetAllSet();
            var offers = await offersSet.Where(offer => offer.CompanyProfileId == profileId)
                .OrderByDescending(offer => offer.CreatedAt)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .Include(offer => offer.ContactDetails)
                .ThenInclude(details => details.ContactType)
                .ToListAsync();

            var display = new JobOffersDisplayCompanyViewModel();

            var offersDisplay = new List<JobOfferCardDisplayCompanyViewModel>();
            foreach (var offer in offers)
            {
                var displayCard = GetDisplayCard(offer, profile)!;

                var cardDisplayCompany = new JobOfferCardDisplayCompanyViewModel()
                {
                    CardDisplay = displayCard,
                    MainTechnology = offer.MainTechnologyType.Type,
                    ContactType = offer.ContactDetails.ContactType.Type,
                    ContactAddress = offer.ContactDetails.ContactAddress,
                    IsPublished = offer.IsPublished,
                };

                offersDisplay.Add(cardDisplayCompany);
            }

            display.JobOffers = offersDisplay;

            return display;
        }

        private JobOfferCardDisplayViewModel GetDisplayCard(JobOffer offer, CompanyProfile profile)
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
                Company = profile.CompanyName,
                CompanyImageUrl = profile.ProfileImageUrl,
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
