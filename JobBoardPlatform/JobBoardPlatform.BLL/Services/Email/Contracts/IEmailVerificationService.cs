namespace JobBoardPlatform.BLL.Services.IdentityVerification.Contracts
{
    internal interface IEmailVerificationService
    {
        void SendVerificationEmail(string email);
    }
}
