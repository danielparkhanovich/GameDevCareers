using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password.Tokens
{
    public interface IRestorePasswordTokensService
    {
        Task<RestorePasswordToken> RegisterNewTokenAsync(string userLogin);
        Task<RestorePasswordToken> TryGetTokenAsync(string tokenId);
    }
}
