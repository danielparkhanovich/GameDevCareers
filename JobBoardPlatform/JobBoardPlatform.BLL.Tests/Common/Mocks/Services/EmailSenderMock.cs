using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.Services
{
    internal class EmailSenderMock : IEmailSender
    {
        public Task SendEmailAsync(string targetEmail, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
