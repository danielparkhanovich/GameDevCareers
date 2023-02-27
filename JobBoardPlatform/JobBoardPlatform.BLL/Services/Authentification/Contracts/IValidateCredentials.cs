using JobBoardPlatform.BLL.Utilities;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface IValidateCredentials
    {
        Task<AuthorizationResult> ValidateRegistration(string email);
        Task<AuthorizationResult> ValidateLogin(string email, string hashedPassword);
    }
}
