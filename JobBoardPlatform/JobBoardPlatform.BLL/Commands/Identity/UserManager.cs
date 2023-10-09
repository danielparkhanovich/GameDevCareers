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
        private readonly IDeleteCommandFactory deleteCommandFactory;


        public UserManager(
            IRepository<T> userRepository, 
            IdentityQueryExecutor<T> queryExecutor,
            IDeleteCommandFactory deleteCommandFactory)
        {
            this.userRepository = userRepository;
            this.queryExecutor = queryExecutor;
            this.deleteCommandFactory = deleteCommandFactory;
        }

        public async Task AddAsync(T identity)
        {
            var addNewUserCommand = new AddNewUserCommand<T>(userRepository, identity);
            await addNewUserCommand.Execute();
        }

        public Task<T> GetAsync(int id)
        {
            return queryExecutor.GetAsync(id);
        }

        public Task<T> GetWithEmailAsync(string email)
        {
            return queryExecutor.GetWithLoginAsync(email);
        }

        public Task<T> GetLoadedAsync(int id)
        {
            return queryExecutor.GetLoadedAsync(id);
        }

        public Task DeleteAsync(int id)
        {
            ICommand deleteCommand = deleteCommandFactory.GetCommand(typeof(T), id);
            return deleteCommand.Execute();
        }
    }
}
