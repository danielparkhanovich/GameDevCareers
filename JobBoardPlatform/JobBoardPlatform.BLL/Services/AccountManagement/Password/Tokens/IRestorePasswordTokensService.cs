using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password.Tokens
{
    public interface IRegistrationTokensService
    {
        Task<RegistrationToken> RegisterNewTokenAsync(string userLogin, string password);
        Task<RegistrationToken> TryGetTokenAsync(string tokenId);
    }
}
