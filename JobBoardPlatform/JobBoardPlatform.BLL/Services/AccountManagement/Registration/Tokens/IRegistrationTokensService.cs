using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public interface IRegistrationTokensService
    {
        Task<RegistrationToken> RegisterNewTokenAsync(string userLogin, string password);
        Task<RegistrationToken> TryGetTokenAsync(string tokenId);
        Task ExpireTokenAsync(string tokenId);
    }
}
