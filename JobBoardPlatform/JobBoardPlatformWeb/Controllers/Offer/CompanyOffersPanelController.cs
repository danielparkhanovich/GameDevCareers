using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Common;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer.CompanyBoard;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    //TODO: Split + add security
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOffersPanelController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IMapper<NewOfferViewModel, JobOffer> viewModelToOffer;


        public CompanyOffersPanelController(IRepository<JobOffer> offersRepository, 
            IRepository<TechKeyword> keywordsRepository,
            IRepository<OfferApplication> applicationsRepository)
        {
            this.offersRepository = offersRepository;
            this.applicationsRepository = applicationsRepository;
            this.viewModelToOffer = new NewOfferViewModelToJobOfferMapper(keywordsRepository);
        }

        public async virtual Task<IActionResult> Offers()
        {
            int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
            var viewModelFactory = new CompanyOffersViewModelFactory(profileId, offersRepository);

            var model = await viewModelFactory.Create();

            return View(model);
        }

        public async virtual Task<IActionResult> AddOffer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> AddOffer(NewOfferViewModel viewModel)
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

        [Route("CompanyOffersPanel/applications-{offerId}-page-{page}")]
        public async virtual Task<IActionResult> Applications(int offerId, int page)
        {
            int pageSize = 10;

            var applicationsSet = await applicationsRepository.GetAllSet();
            var companyApplications = applicationsSet.Where(application => application.JobOfferId == offerId);

            int totalApplications = companyApplications.Count();
            int applicationsAfterFilters = 0;

            var applications = await companyApplications
                .Include(application => application.EmployeeProfile)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var offersSet = await offersRepository.GetAllSet();

            var offer = offersSet.Where(x => x.Id == offerId)
                .Include(x => x.CompanyProfile)
                .Include(x => x.WorkLocation)
                .Include(x => x.MainTechnologyType)
                .Include(x => x.TechKeywords)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.SalaryRange != null ? y.SalaryRange.SalaryCurrency : null)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.EmploymentType)
                .Include(x => x.ContactDetails)
                    .ThenInclude(y => y.ContactType)
                .Single();

            var applicationsViewModel = new CompanyApplicationsViewModel();

            var applicationCards = new List<CompanyApplicationCardViewModel>(applications.Count);
            var daysFormatter = new DaysFormatter(true);

            foreach (var application in applications)
            {
                string applicatedAgo = daysFormatter.GetDaysAgoString(application.CreatedAt);
                string? linkedInUrl = application.EmployeeProfile?.LinkedInUrl;
                string? profileImageUrl = application.EmployeeProfile?.ProfileImageUrl;
                string? yearsOfExperience = application.EmployeeProfile?.YearsOfExperience;
                string? city = application.EmployeeProfile?.City;
                string? country = application.EmployeeProfile?.Country;

                applicationCards.Add(new CompanyApplicationCardViewModel()
                {
                    Id = application.Id,
                    PriorityFlagId = application.ApplicationFlagTypeId,
                    FullName = application.FullName,
                    Email = application.Email,
                    ProfileImageUrl = profileImageUrl,
                    ResumeUrl = application.ResumeUrl,
                    YearsOfExperience = yearsOfExperience,
                    Description = application.Description,
                    ApplicatedAgo = applicatedAgo,
                    LinkedInUrl = linkedInUrl,
                    City = city,
                    Country = country
                });
            }
            applicationsViewModel.Applications = applicationCards;

            var offerToCardViewModel = new JobOfferToOfferViewModelMapper();
            var displayCard = new OfferCardViewModel();
            offerToCardViewModel.Map(offer, displayCard);
            applicationsViewModel.OfferCard = displayCard;

            applicationsViewModel.TotalApplications = totalApplications;
            applicationsViewModel.AfterFiltersApplications = applicationCards.Count;
            applicationsViewModel.TotalViewsCount = offer.NumberOfViews;
            applicationsViewModel.Page = page;

            return View(applicationsViewModel);
        }

        [HttpPost]
        public async virtual Task<IActionResult> SetPriority(int applicationId, int priorityIndex)
        {
            var application = await applicationsRepository.Get(applicationId);

            // double select -> deselect
            if (application.ApplicationFlagTypeId == priorityIndex)
            {
                application.ApplicationFlagTypeId = 1;
            }
            else
            {
                application.ApplicationFlagTypeId = priorityIndex;
            }

            await applicationsRepository.Update(application);

            string message = "SUCCESS";
            return Json(new { Message = message, Priority = application.ApplicationFlagTypeId });
        }
    }
}
