using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.CompanyPanel.Applications;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.Controllers.Presenters;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.PL.ViewModels.Models.Email;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Route("applications")]
    [Authorize(Policy = AuthorizationPolicies.OfferOwnerOnlyPolicy)]
    public class CompanyApplicationsPanelController : CardsControllerBase
    {
        public const string SetPriorityAction = "SetPriority";

        private readonly OfferApplicationsSearcher searcher;
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IApplicationsManager applicationsManager;
        private readonly IViewRenderService viewRenderService;


        public CompanyApplicationsPanelController(
            OfferApplicationsSearcher searcher, 
            IOfferQueryExecutor queryExecutor,
            IApplicationsManager applicationsManager,
            IViewRenderService viewRenderService)
        {
            this.searcher = searcher;
            this.queryExecutor = queryExecutor;
            this.applicationsManager = applicationsManager;
            this.viewRenderService = viewRenderService;
        }

        [Route("offer")]
        public async virtual Task<IActionResult> Applications(int offerId)
        {
            var applicationsViewModelFactory = new CompanyApplicationsViewModelFactory(
                searcher, GetSearchParams(), queryExecutor);
            var applicationsViewModel = await applicationsViewModelFactory.CreateAsync();

            return View(applicationsViewModel);
        }

        [HttpPost(SetPriorityAction)]
        public async virtual Task<IActionResult> SetPriority(int applicationId, int priorityIndex, int offerId)
        {
            int resultPriority = await applicationsManager.UpdateApplicationPriorityAsync(
                applicationId, priorityIndex);

            string message = "SUCCESS";
            return Json(new { Message = message, Priority = resultPriority });
        }

        [Route("preview")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
        public async Task<IActionResult> PreviewApplicationEmail(int applicationId)
        {
            viewRenderService.SetController(this);

            var application = await applicationsManager.GetApplicationAsync(applicationId);
            ViewBag.EmailHtml = await viewRenderService.RenderPartialViewToString(
                ApplicationEmailViewModel.EmailView, application);
            return View();
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
            var searchParams = searchParamsFactory.GetSearchParams(Request);
            searchParams.OfferId = GetParsedOfferId();
            return searchParams;
        }

        private int GetParsedOfferId()
        {
            string? offerIdString = Request.RouteValues[OfferSearchUrlParams.OfferId]?.ToString();
            if (string.IsNullOrEmpty(offerIdString))
            {
                offerIdString = Request.Query[OfferSearchUrlParams.OfferId];
            }
            return int.Parse(offerIdString!);
        }
    }
}
