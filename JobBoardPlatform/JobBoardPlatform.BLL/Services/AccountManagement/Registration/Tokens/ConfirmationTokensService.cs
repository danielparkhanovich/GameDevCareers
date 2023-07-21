using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    /// <summary>
    /// Accepts (confirmation token id, id to confirm)
    /// </summary>
    public class ConfirmationTokensService : TokensServiceBase<ConfirmationToken, (string, string)> 
    {
        private const string TokenEntryKeyPrefix = "ConfirmationToken";


        public ConfirmationTokensService(ICacheRepository<ConfirmationToken> cache) : base(cache)
        {

        }

        protected override ConfirmationToken CreateNewToken((string, string) data, string tokenId)
        {
            return new ConfirmationToken()
            {
                Id = data.Item1,
                TokenToConfirmId = data.Item2,
            };
        }

        protected override string GetTokenEntryKeyPrefix()
        {
            return TokenEntryKeyPrefix;
        }
    }
}
