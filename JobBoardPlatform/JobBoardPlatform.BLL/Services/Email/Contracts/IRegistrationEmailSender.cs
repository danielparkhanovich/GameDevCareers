namespace JobBoardPlatform.BLL.Services.IdentityVerification.Contracts
{
    public interface IRegistrationEmailSender
    {
        Task SendEmailAsync(string targetEmail, string confirmationUrl);
    }
}
