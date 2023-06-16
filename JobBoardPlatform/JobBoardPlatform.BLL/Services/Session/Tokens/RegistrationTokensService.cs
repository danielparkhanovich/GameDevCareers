using JobBoardPlatform.BLL.Services.Session.Contracts;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.Session.Tokens
{
    public class RegistrationTokensService : IRegistrationTokensService
    {
        private const string RegistrationTokenEntryKeyPrefix = "RegistrationToken";
        private readonly ICacheRepository<RegistrationToken> cache;


        public RegistrationTokensService(ICacheRepository<RegistrationToken> cache)
        {
            this.cache = cache;
        }

        public async Task<RegistrationToken> RegisterNewTokenAsync(string userLogin)
        {
            var token = CreateNewToken(userLogin);

            string entryKey = GetTokenEntryKey(token.Id);
            await cache.UpdateAsync(entryKey, token);

            return token;
        }

        public async Task<RegistrationToken> TryGetTokenAsync(string tokenId)
        {
            string entryKey = GetTokenEntryKey(tokenId);
            return await TryGetTokenFromCacheAsync(entryKey);
        }

        private async Task<RegistrationToken> TryGetTokenFromCacheAsync(string entryKey)
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

        private RegistrationToken CreateNewToken(string userLogin)
        {
            return new RegistrationToken() 
            { 
                Id = GetTokenId(),
                RelatedLogin = userLogin,
            };
        }

        private string GetTokenId()
        {
            return Guid.NewGuid().ToString();
        }

        private string GetTokenEntryKey(string tokenId)
        {
            return $"{RegistrationTokenEntryKeyPrefix}_{tokenId}";
        }
    }
}
