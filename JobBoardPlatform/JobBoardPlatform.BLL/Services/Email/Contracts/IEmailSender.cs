namespace JobBoardPlatform.BLL.Services.IdentityVerification.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string targetEmail, string subject, string message);
    }
}
