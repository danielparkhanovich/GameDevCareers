using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Offer.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{    
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyApplicationsPanelController : Controller
    {
        private const int PageSize = 10;

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
            bool[] filterStates = new bool[4] { isShowUnseen, isShowMustHire, isShowAverage, isShowRejected };

            var applicationsViewModelFactory = new CompanyApplicationsViewModelFactory(offerId,
                page,
                PageSize,
                filterStates,
                applicationsRepository,
                offersRepository);

            var applicationsViewModel = await applicationsViewModelFactory.Create();

            return View(applicationsViewModel);
        }

        [HttpPost]
        public async virtual Task<IActionResult> SetPriority(int applicationId, int priorityIndex)
        {
            var updateApplicationPriorityCommand = new UpdateApplicationPriorityCommand(applicationsRepository, 
                applicationId, 
                priorityIndex);
            await updateApplicationPriorityCommand.Execute();

            var resultPriorityIndex = updateApplicationPriorityCommand.ResultPriorityIndex;

            string message = "SUCCESS";
            return Json(new { Message = message, Priority = resultPriorityIndex });
        }

        [HttpPost]
        public async virtual Task<IActionResult> RefreshApplications(CompanyApplicationsCardsViewModel cardsViewModel)
        {
            bool[] filterStates = new bool[4] 
            { 
                cardsViewModel.IsIncludeUnseen,
                cardsViewModel.IsIncludeMustHire,
                cardsViewModel.IsIncludeAverage,
                cardsViewModel.IsIncludeReject
            };

            var applicationCardsFactory = new CompanyApplicationsCardsViewModelFactory(cardsViewModel.OfferId,
                applicationsRepository,
                cardsViewModel.Page,
                PageSize,
                filterStates,
                cardsViewModel.SortType,
                cardsViewModel.SortCategory);

            var applicationCards = await applicationCardsFactory.Create();

            return PartialView("./Applications/_ApplicationsContainer", applicationCards);
        }
    }
}
