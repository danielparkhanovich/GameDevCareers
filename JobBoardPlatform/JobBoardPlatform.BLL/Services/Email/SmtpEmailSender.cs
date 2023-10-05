using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace JobBoardPlatform.BLL.Services.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailConfiguration options;
        private readonly SmtpClient client;


        public SmtpEmailSender(IOptions<EmailConfiguration> options)
        {
            this.options = options.Value;
            this.client = GetSmtpClient(options.Value);
        }

        public Task SendEmailAsync(string targetEmail, string subject, string message)
        {
            var mail = GetMailMessage(targetEmail, subject, message);
            return client.SendMailAsync(mail);
        }

        private SmtpClient GetSmtpClient(EmailConfiguration options)
        {
            return new SmtpClient(options.Server, options.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(options.SourceEmail, options.Password)
            };
        }

        private MailMessage GetMailMessage(string targetEmail, string subject, string message)
        {
            var mail = new MailMessage(options.SourceEmail, targetEmail, subject, message);
            mail.IsBodyHtml = true;
            return mail;
        }
    }
}
