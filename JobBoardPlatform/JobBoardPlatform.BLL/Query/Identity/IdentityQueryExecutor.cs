using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Query.Identity
{
    internal class IdentityQueryExecutor<T> where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> repository;


        public IdentityQueryExecutor(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public async Task<T> GetIdentityByEmail(string login)
        {
            var query = new GetIdentityByEmailQuery<T>(repository, login);
            var user = await query.Get();
            return user;
        }
    }
}
