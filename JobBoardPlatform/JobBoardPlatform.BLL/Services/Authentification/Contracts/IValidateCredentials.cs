using JobBoardPlatform.BLL.Utilities;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface IValidateCredentials
    {
        Task<AuthorizationResult> ValidateRegisterAsync(string email);
        Task<AuthorizationResult> ValidateLoginAsync(string email, string hashedPassword);
    }
}
