using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
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
            bool[] filterToggles = new bool[4] { true, true, true, true };

            var applicationsViewModelFactory = new CompanyApplicationsViewModelFactory(offerId,
                page,
                PageSize,
                filterToggles,
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
        public async virtual Task<IActionResult> RefreshCardContainer(ContainerCardsViewModel cardsViewModel)
        {
            var applicationCardsFactory = new CompanyApplicationsCardsViewModelFactory(cardsViewModel.RelatedId,
                applicationsRepository,
                cardsViewModel.Page,
                PageSize,
                cardsViewModel.FilterToggles,
                cardsViewModel.SortType,
                cardsViewModel.SortCategory);

            var applicationCards = await applicationCardsFactory.Create();

            return PartialView("./Templates/_CardsContainer", applicationCards);
        }
    }
}
