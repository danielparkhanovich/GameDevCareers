using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Common;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    //TODO: Split + add security (Applications -> separate controller)
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyApplicationsPanelController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<OfferApplication> applicationsRepository;


        public CompanyApplicationsPanelController(IRepository<JobOffer> offersRepository, 
            IRepository<OfferApplication> applicationsRepository)
        {
            this.offersRepository = offersRepository;
            this.applicationsRepository = applicationsRepository;
        }

        [Route("CompanyOffersPanel/applications-{offerId}-page-{page}")]
        public async virtual Task<IActionResult> Applications(int offerId, int page)
        {
            bool isShowUnseen = !Request.Query.ContainsKey("hide-unseen");
            bool isShowMustHire = !Request.Query.ContainsKey("hide-musthire");
            bool isShowAverage = !Request.Query.ContainsKey("hide-average");
            bool isShowRejected = !Request.Query.ContainsKey("hide-reject");

            string sortBy = null;
            if (sortBy == null)
            {
                sortBy = "date";
            }

            int pageSize = 10;

            var applicationsSet = await applicationsRepository.GetAllSet();
            var companyApplications = applicationsSet.Where(application => application.JobOfferId == offerId &&
                    ((application.ApplicationFlagTypeId == 1 && isShowUnseen) ||
                     (application.ApplicationFlagTypeId == 2 && isShowMustHire) ||
                     (application.ApplicationFlagTypeId == 3 && isShowAverage) || 
                     (application.ApplicationFlagTypeId == 4 && isShowRejected))
                     );

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

            applicationsViewModel.SortBy = sortBy;
            applicationsViewModel.IsIncludeUnseen = isShowUnseen;
            applicationsViewModel.IsIncludeMustHire = isShowMustHire;
            applicationsViewModel.IsIncludeAverage = isShowAverage;
            applicationsViewModel.IsIncludeReject = isShowRejected;

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
