
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IEmailCompanyRegistrationService
    {
        Task TrySendConfirmationTokenAndPasswordAsync(string email, string password);
        Task TryRegisterByTokenAsync(string tokenId, HttpContext httpContext);
    }
}
