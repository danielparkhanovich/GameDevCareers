
namespace JobBoardPlatform.BLL.Services.IdentityVerification.Contracts
{
    public interface IResetPasswordEmailSender
    {
        Task SendEmailAsync(string targetEmail, string resetUrl);
    }
}
