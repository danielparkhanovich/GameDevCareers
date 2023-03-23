namespace JobBoardPlatform.BLL.Services.IdentityVerification.Contracts
{
    internal interface IEmailService
    {
        void SendVerificationEmail(string email);
    }
}
