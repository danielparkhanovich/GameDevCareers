using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Controllers.Utils.Renderers;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Email;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Utils
{
    public class ApplicationEmailViewRenderer : IEmailContent<JobOfferApplication>
    {
        private readonly IViewRenderService viewRenderService;
        private readonly IOfferManager offerManager;
        private Controller? controller;


        public ApplicationEmailViewRenderer(IViewRenderService viewRenderService, IOfferManager offerManager)
        {
            this.viewRenderService = viewRenderService;
            this.offerManager = offerManager;
        }

        public void SetController(Controller controller)
        {
            this.controller = controller;
        }

        public async Task<string> GetSubjectAsync(JobOfferApplication application)
        {
            var offer = await offerManager.GetAsync(application.JobOfferId);
            return $"New Job Application - {offer.JobTitle} at {offer.CompanyProfile.CompanyName}";
        }

        public async Task<string> GetMessageAsync(JobOfferApplication application)
        {
            var offer = await offerManager.GetAsync(application.JobOfferId);

            var cardFactory = new CompanyApplicationCardViewModelFactory();
            var model = new ApplicationEmailViewModel()
            {
                CompanyName = offer.CompanyProfile.CompanyName,
                JobTitle = offer.JobTitle,
                Application = (cardFactory.CreateCard(application) as ApplicationCardViewModel)!
            };

            var emailHtml = await viewRenderService.RenderPartialViewToString(
                controller!, model.Application.EmailView, model);
            return emailHtml;
        }
    }
}