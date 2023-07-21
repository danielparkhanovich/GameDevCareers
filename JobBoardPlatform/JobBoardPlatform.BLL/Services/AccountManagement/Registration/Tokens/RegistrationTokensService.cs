using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    /// <summary>
    /// Accepts (userLogin, password)
    /// </summary>
    public class RegistrationTokensService : TokensServiceBase<RegistrationToken, (string, string)>, IRegistrationTokensService
    {
        private const string RegistrationTokenEntryKeyPrefix = "RegistrationToken";
        private readonly IPasswordHasher passwordHasher;


        public RegistrationTokensService(
            ICacheRepository<RegistrationToken> cache,
            IPasswordHasher passwordHasher) : base(cache)
        {
            this.passwordHasher = passwordHasher;
        }

        public Task<RegistrationToken> RegisterNewTokenAsync(string userLogin, string password)
        {
            return base.RegisterNewTokenAsync((userLogin, password));
        }

        protected override RegistrationToken CreateNewToken((string, string) data, string tokenId)
        {
            return new RegistrationToken()
            {
                Id = tokenId,
                RelatedLogin = data.Item1,
                PasswordHash = passwordHasher.GetHash(data.Item2)
            };
        }

        protected override string GetTokenEntryKeyPrefix()
        {
            return RegistrationTokenEntryKeyPrefix;
        }
    }
}
