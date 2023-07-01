using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public interface IRestorePasswordTokensService
    {
        Task<RestorePasswordToken> RegisterNewTokenAsync(string userLogin);
        Task<RestorePasswordToken> TryGetTokenAsync(string tokenId);
    }
}
