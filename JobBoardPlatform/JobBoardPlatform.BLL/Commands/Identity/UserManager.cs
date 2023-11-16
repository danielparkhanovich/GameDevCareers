using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identity
{
    public class UserManager<T> where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> userRepository;
        private readonly IdentityQueryExecutor<T> queryExecutor;
        private readonly IDeleteUserCommandFactory deleteCommandFactory;
        private readonly IUpdateUserCommandFactory updateCommandFactory;


        public UserManager(
            IRepository<T> userRepository, 
            IdentityQueryExecutor<T> queryExecutor,
            IDeleteUserCommandFactory deleteCommandFactory,
            IUpdateUserCommandFactory updateCommandFactory)
        {
            this.userRepository = userRepository;
            this.queryExecutor = queryExecutor;
            this.deleteCommandFactory = deleteCommandFactory;
            this.updateCommandFactory = updateCommandFactory;
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

        public async Task<List<T>> GetAllAsync()
        {
            var users = await userRepository.GetAll();
            return users;
        }

        public async Task<List<T>> GetAllLoadedAsync()
        {
            var loaded = new List<T>();

            var users = await userRepository.GetAll();
            var usersIds = users.Select(x => x.Id);
            foreach (var id in usersIds)
            {
                loaded.Add(await queryExecutor.GetAsync(id));
            }

            return loaded;
        }

        public Task<T> GetWithEmailAsync(string email)
        {
            return queryExecutor.GetWithLoginAsync(email);
        }

        public Task<T> GetLoadedAsync(int id)
        {
            return queryExecutor.GetLoadedAsync(id);
        }

        public async Task<bool> IsExistsWithEmailAsync(string email)
        {
            return (await GetWithEmailAsync(email)) != null;
        }

        public async Task UpdateProfileAsync(int id, ProfileData profileData)
        {
            var user = await GetAsync(id);

            var command = updateCommandFactory.GetCommand(typeof(T), user.ProfileId, profileData);
            await command.Execute();
        }

        public async Task DeleteAsync(int id)
        {
            var command = deleteCommandFactory.GetCommand(typeof(T), id);
            await command.Execute();
        }
    }
}
