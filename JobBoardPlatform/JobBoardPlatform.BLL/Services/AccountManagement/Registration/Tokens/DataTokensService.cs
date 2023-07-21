using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public class DataTokensService<T> : TokensServiceBase<DataToken<T>, T>
    {
        private const string TokenEntryKeyPrefix = "DataToken";


        public DataTokensService(ICacheRepository<DataToken<T>> cache) : base(cache)
        {

        }

        protected override DataToken<T> CreateNewToken(T data, string tokenId)
        {
            return new DataToken<T>()
            {
                Id = tokenId,
                Value = data,
                IsConfirmed = false,
            };
        }

        protected override string GetTokenEntryKeyPrefix()
        {
            return TokenEntryKeyPrefix;
        }
    }
}
