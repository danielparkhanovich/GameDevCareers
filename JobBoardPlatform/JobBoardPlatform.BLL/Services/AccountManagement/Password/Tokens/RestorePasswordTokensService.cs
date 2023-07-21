using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password.Tokens
{
    public class RestorePasswordTokensService : TokensServiceBase<RestorePasswordToken, string>, IRestorePasswordTokensService
    {
        private const string RestorePasswordTokenEntryKeyPrefix = "RestorePasswordToken";


        public RestorePasswordTokensService(ICacheRepository<RestorePasswordToken> cache) : base(cache)
        {

        }

        public new Task<RestorePasswordToken> RegisterNewTokenAsync(string userLogin)
        {
            return base.RegisterNewTokenAsync(userLogin);
        }

        protected override RestorePasswordToken CreateNewToken(string relatedLogin, string tokenId)
        {
            return new RestorePasswordToken()
            {
                Id = tokenId,
                RelatedLogin = relatedLogin,
            };
        }

        protected override string GetTokenEntryKeyPrefix()
        {
            return RestorePasswordTokenEntryKeyPrefix;
        }
    }
}
