using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Email;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;

namespace JobBoardPlatform.PL.Controllers.Presenters
{
    public class ApplicationsEmailSender : IApplicationsEmailSender
    {
        private readonly IViewRenderService viewRenderService;
        private readonly IEmailSender emailSender;
        private readonly IOfferManager offerManager;


        public ApplicationsEmailSender(
            IViewRenderService viewRenderService, 
            IEmailSender emailSender,
            IOfferManager offerManager)
        {
            this.viewRenderService = viewRenderService;
            this.emailSender = emailSender;
            this.offerManager = offerManager;
        }

        public async Task SendEmailAsync(string targetEmail, JobOfferApplication application)
        {
            var subject = await GetSubjectAsync(application);
            var message = await GetMessageAsync(application);
            await emailSender.SendEmailAsync(targetEmail, subject, message);
        }

        private async Task<string> GetSubjectAsync(JobOfferApplication application)
        {
            var offer = await offerManager.GetAsync(application.JobOfferId);
            return $"New Job Application - {offer.JobTitle} at {offer.CompanyProfile.CompanyName}";
        }

        private async Task<string> GetMessageAsync(JobOfferApplication application)
        {
            var offer = await offerManager.GetAsync(application.JobOfferId);

            var cardFactory = new CompanyApplicationCardViewModelFactory();
            var model = new ApplicationEmailViewModel()
            {
                CompanyName = offer.CompanyProfile.CompanyName,
                JobTitle = offer.JobTitle,
                Application = (cardFactory.CreateCard(application) as ApplicationCardViewModel)!
            };

            var emailHtml = await viewRenderService.RenderPartialViewToString(ApplicationEmailViewModel.EmailView, model);
            return emailHtml;
        }
    }
}