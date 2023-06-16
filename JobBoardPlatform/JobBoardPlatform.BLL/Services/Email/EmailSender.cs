using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace JobBoardPlatform.BLL.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration options;


        public EmailSender(IOptions<EmailConfiguration> options)
        {
            this.options = options.Value;
        }

        public Task SendEmailAsync(string targetEmail, string subject, string message)
        {
            var client = GetSmtpClient();
            return client.SendMailAsync(new MailMessage(options.SourceEmail, targetEmail, subject, message));
        }

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient(options.Server, options.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(options.SourceEmail, options.Password)
            };
        }
    }
}
