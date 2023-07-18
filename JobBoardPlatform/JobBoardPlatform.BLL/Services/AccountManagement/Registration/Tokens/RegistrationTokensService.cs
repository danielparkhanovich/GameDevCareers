using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Session.Tokens;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public class RegistrationTokensService : IRegistrationTokensService
    {
        private const string RegistrationTokenEntryKeyPrefix = "RegistrationToken";
        private readonly ICacheRepository<RegistrationToken> cache;
        private readonly IPasswordHasher passwordHasher;


        public RegistrationTokensService(
            ICacheRepository<RegistrationToken> cache,
            IPasswordHasher passwordHasher)
        {
            this.cache = cache;
            this.passwordHasher = passwordHasher;
        }

        public async Task<RegistrationToken> RegisterNewTokenAsync(string userLogin, string password)
        {
            var token = CreateNewToken(userLogin, password);

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

        private RegistrationToken CreateNewToken(string userLogin, string password)
        {
            return new RegistrationToken() 
            { 
                Id = GetTokenId(),
                RelatedLogin = userLogin,
                PasswordHash = passwordHasher.GetHash(password),
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
