using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.Session.Contracts
{
    public interface IRegistrationTokensService
    {
        Task<RegistrationToken> RegisterNewTokenAsync(string userLogin);
        Task<RegistrationToken> TryGetTokenAsync(string tokenId);
    }
}
