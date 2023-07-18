using JobBoardPlatform.BLL.Services.Session.Tokens;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public class DataTokensService<T>
    {
        private const string TokenEntryKeyPrefix = "DataToken";
        private readonly ICacheRepository<DataToken<T>> cache;


        public DataTokensService(ICacheRepository<DataToken<T>> cache)
        {
            this.cache = cache;
        }

        public async Task<DataToken<T>> RegisterNewTokenAsync(T data)
        {
            var token = CreateNewToken(data);

            string entryKey = GetTokenEntryKey(token.Id);
            await cache.UpdateAsync(entryKey, token);

            return token;
        }

        public async Task<DataToken<T>> TryGetTokenAsync(string tokenId)
        {
            string entryKey = GetTokenEntryKey(tokenId);
            return await TryGetTokenFromCacheAsync(entryKey);
        }

        public async Task ExpireTokenAsync(string tokenId)
        {
            await cache.DeleteAsync(tokenId);
        }

        private async Task<DataToken<T>> TryGetTokenFromCacheAsync(string entryKey)
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

        private DataToken<T> CreateNewToken(T data)
        {
            return new DataToken<T>() 
            { 
                Id = GetTokenId(),
                Value = data
            };
        }

        private string GetTokenId()
        {
            return Guid.NewGuid().ToString();
        }

        private string GetTokenEntryKey(string tokenId)
        {
            return $"{TokenEntryKeyPrefix}_{tokenId}";
        }
    }
}
