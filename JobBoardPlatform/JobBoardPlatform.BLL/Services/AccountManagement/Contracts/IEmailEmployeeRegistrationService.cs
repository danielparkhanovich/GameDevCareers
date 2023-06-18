using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IEmailEmployeeRegistrationService
    {
        Task TrySendConfirmationTokenAsync(string email, string password);
        Task TryRegisterByTokenAsync(string tokenId, HttpContext httpContext);
    }
}
