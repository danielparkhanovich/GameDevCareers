using JobBoardPlatform.BLL.Services.Authorization.Utilities;

namespace JobBoardPlatform.BLL.Services.Authorization.Contracts
{
    public interface IAuthorizationService
    {
        Task SignInHttpContextAsync(AuthorizationData data);
        Task SignOutHttpContextAsync();
    }
}
