using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identity
{
    public class UserManager<T> where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> userRepository;
        private readonly IdentityQueryExecutor<T> queryExecutor;


        public UserManager(IRepository<T> userRepository, IdentityQueryExecutor<T> queryExecutor)
        {
            this.userRepository = userRepository;
            this.queryExecutor = queryExecutor;
        }

        public async Task AddNewUser(T identity)
        {
            var addNewUserCommand = new AddNewUserCommand<T>(userRepository, identity);
            await addNewUserCommand.Execute();
        }

        public Task<T> GetUserByEmail(string email)
        {
            return queryExecutor.GetIdentityByEmail(email);
        }
    }
}
