using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Session.Tokens;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Common
{
    public abstract class TokensServiceBase<TToken, TValue> where TToken : IToken
    {
        private readonly ICacheRepository<TToken> cache;


        public TokensServiceBase(ICacheRepository<TToken> cache)
        {
            this.cache = cache;
        }

        public async Task<TToken> RegisterNewTokenAsync(TValue data)
        {
            string tokenId = GetTokenId();
            var token = CreateNewToken(data, tokenId);

            string entryKey = GetTokenEntryKey(token.Id);
            await cache.UpdateAsync(entryKey, token);

            return token;
        }

        public async Task<TToken> TryGetTokenAsync(string tokenId)
        {
            string entryKey = GetTokenEntryKey(tokenId);
            return await TryGetTokenFromCacheAsync(entryKey);
        }

        public async Task ExpireTokenAsync(string tokenId)
        {
            string entryKey = GetTokenEntryKey(tokenId);
            await cache.DeleteAsync(entryKey);
        }

        protected abstract TToken CreateNewToken(TValue data, string tokenId);

        protected abstract string GetTokenEntryKeyPrefix();

        protected virtual string GetTokenId()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task<TToken> TryGetTokenFromCacheAsync(string entryKey)
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

        private string GetTokenEntryKey(string tokenId)
        {
            return $"{GetTokenEntryKeyPrefix()}_{tokenId}";
        }
    }
}
