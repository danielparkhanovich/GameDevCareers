using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Session.Tokens;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password.Tokens
{
    public class RestorePasswordTokensService : IRestorePasswordTokensService
    {
        private const string RestorePasswordTokenEntryKeyPrefix = "RestorePasswordToken";
        private readonly ICacheRepository<RestorePasswordToken> cache;


        public RestorePasswordTokensService(ICacheRepository<RestorePasswordToken> cache)
        {
            this.cache = cache;
        }

        public async Task<RestorePasswordToken> RegisterNewTokenAsync(string userLogin)
        {
            var token = CreateNewToken(userLogin);

            string entryKey = GetTokenEntryKey(token.Id);
            await cache.UpdateAsync(entryKey, token);

            return token;
        }

        public async Task<RestorePasswordToken> TryGetTokenAsync(string tokenId)
        {
            string entryKey = GetTokenEntryKey(tokenId);
            return await TryGetTokenFromCacheAsync(entryKey);
        }

        private async Task<RestorePasswordToken> TryGetTokenFromCacheAsync(string entryKey)
        {
            try
            {
                return await cache.GetAsync(entryKey);
            }
            catch (CacheEntryException e)
            {
                throw new TokenValidationException(e.Message);
            }
        }

        private RestorePasswordToken CreateNewToken(string userLogin)
        {
            return new RestorePasswordToken()
            {
                Id = GetTokenId(),
                RelatedLogin = userLogin
            };
        }

        private string GetTokenId()
        {
            return Guid.NewGuid().ToString();
        }

        private string GetTokenEntryKey(string tokenId)
        {
            return $"{RestorePasswordTokenEntryKeyPrefix}_{tokenId}";
        }
    }
}
