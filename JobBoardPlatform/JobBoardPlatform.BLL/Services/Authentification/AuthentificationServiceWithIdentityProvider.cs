using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    /// <summary>
    /// Example providers: Google/Facebook...
    /// </summary>
    public class AuthentificationServiceWithIdentityProvider<T> : IAuthentificationService<T> 
        where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> identityRepository;
        private readonly IIdentityValidator<T> identityValidator;
        private readonly IdentityQueryExecutor<T> identityQuery;


        public AuthentificationServiceWithIdentityProvider(IRepository<T> repository)
        {
            this.identityRepository = repository;
            this.identityValidator = new IdentityValidator<T>();
            this.identityQuery = new IdentityQueryExecutor<T>(repository);
        }

        public async Task<T> TryRegisterAsync(T identity)
        {
            var user = await identityQuery.GetIdentityByEmail(identity.Email);
            identityValidator.ValidateRegisterAsync(user);
            return await AddNewUser(identity);
        }

        public async Task<T> TryLoginAsync(T identity)
        {
            var user = await identityQuery.GetIdentityByEmail(identity.Email);
            identityValidator.ValidateLoginAsync(user, user.HashPassword);
            return user;
        }

        private async Task<T> AddNewUser(T identity)
        {
            var addNewUserCommand = new AddNewUserCommand<T>(identityRepository, identity);
            await addNewUserCommand.Execute();
            return addNewUserCommand.AddedRecord;
        }
    }
}
