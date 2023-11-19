using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Email;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;

namespace JobBoardPlatform.PL.Controllers.Presenters
{
    public class ResetPasswordEmailSender : IResetPasswordEmailSender
    {
        private readonly IViewRenderService viewRenderService;
        private readonly IEmailSender emailSender;
        private readonly IOfferManager offerManager;


        public ResetPasswordEmailSender(
            IViewRenderService viewRenderService, 
            IEmailSender emailSender,
            IOfferManager offerManager)
        {
            this.viewRenderService = viewRenderService;
            this.emailSender = emailSender;
            this.offerManager = offerManager;
        }

        public async Task SendEmailAsync(string targetEmail, string resetUrl)
        {
            var subject = GetSubject();
            var message = await GetMessageAsync(resetUrl);
            await emailSender.SendEmailAsync(targetEmail, subject, message);
        }

        private string GetSubject()
        {
            return $"{Configuration.GlobalPL.Configuration.ApplicationName} - Reset Password";
        }

        private async Task<string> GetMessageAsync(string resetUrl)
        {
            var model = new ResetPasswordEmailViewModel()
            {
               ResetPasswordLink = resetUrl,
            };

            var emailHtml = await viewRenderService.RenderPartialViewToString(
                ResetPasswordEmailViewModel.EmailView, model);
            return emailHtml;
        }
    }
}