using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface ISessionManager
    {
        Task SignInHttpContextAsync(ICredentialEntity credentials);
        Task SignOutHttpContextAsync();
    }
}
