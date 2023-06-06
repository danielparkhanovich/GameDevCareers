using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.CompanyPanel.Applications;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
    public class CompanyApplicationsPanelController : CardsControllerBase
    {
        private readonly OfferApplicationsSearcher searcher;
        private readonly OfferQueryExecutor queryExecutor;
        private readonly ApplicationCommandsExecutor commandsExecutor;


        public CompanyApplicationsPanelController(
            OfferApplicationsSearcher searcher, 
            OfferQueryExecutor queryExecutor,
            ApplicationCommandsExecutor commandsExecutor)
        {
            this.searcher = searcher;
            this.queryExecutor = queryExecutor;
            this.commandsExecutor = commandsExecutor;
        }

        [Route("CompanyOffersPanel/applications-{offerId}-page-{page}")]
        public async virtual Task<IActionResult> Applications(int offerId, int page)
        {
            var applicationsViewModelFactory = new CompanyApplicationsViewModelFactory(
                searcher, GetSearchParams(), queryExecutor);
            var applicationsViewModel = await applicationsViewModelFactory.CreateAsync();

            return View(applicationsViewModel);
        }

        [HttpPost]
        public async virtual Task<IActionResult> SetPriority(int applicationId, int priorityIndex)
        {
            int resultPriority = await commandsExecutor.UpdateApplicationPriorityCommandAsync(
                applicationId, priorityIndex);

            string message = "SUCCESS";
            return Json(new { Message = message, Priority = resultPriority });
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var searchParams = GetSearchParams();
            var containerFactory = new CompanyApplicationsContainerViewModelFactory(searcher, searchParams);
            return containerFactory.CreateAsync();
        }

        private CompanyPanelApplicationSearchParams GetSearchParams()
        {
            var searchParamsFactory = new CompanyPanelApplicationSearchParamsFactory();
            return searchParamsFactory.GetSearchParams(Request);
        }
    }
}
