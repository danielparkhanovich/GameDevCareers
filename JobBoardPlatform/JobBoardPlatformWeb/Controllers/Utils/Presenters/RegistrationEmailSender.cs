using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Email;

namespace JobBoardPlatform.PL.Controllers.Presenters
{
    public class RegistrationEmailSender : IRegistrationEmailSender
    {
        private readonly IViewRenderService viewRenderService;
        private readonly IEmailSender emailSender;


        public RegistrationEmailSender(
            IViewRenderService viewRenderService, 
            IEmailSender emailSender)
        {
            this.viewRenderService = viewRenderService;
            this.emailSender = emailSender;
        }

        public async Task SendEmailAsync(string targetEmail, string confirmationUrl)
        {
            var subject = GetSubject();
            var message = await GetMessageAsync(confirmationUrl);
            await emailSender.SendEmailAsync(targetEmail, subject, message);
        }

        private string GetSubject()
        {
            return $"Welcome to {Configuration.GlobalPL.Configuration.ApplicationName} - Confirm Your Registration!";
        }

        private async Task<string> GetMessageAsync(string confirmationLink)
        {
            var model = new RegistrationEmailViewModel()
            {
                ConfirmationLink = confirmationLink
            };

            var emailHtml = await viewRenderService.RenderPartialViewToString(RegistrationEmailViewModel.EmailView, model);
            return emailHtml;
        }
    }
}