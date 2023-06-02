using JobBoardPlatform.BLL.Query;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class ModifyIdentityService<T> : IModifyIdentityService<T>
        where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> identityRepository;
        private readonly IIdentityValidator<T> identityValidator;
        private readonly IPasswordHasher passwordHasher;
        private readonly IdentityQueryExecutor<T> identityQuery;


        public ModifyIdentityService(IRepository<T> repository)
        {
            this.identityRepository = repository;
            this.identityValidator = new IdentityValidator<T>(repository);
            this.passwordHasher = new MD5Hasher();
            this.identityQuery = new IdentityQueryExecutor<T>(repository);
        }

        public async Task<IUserIdentityEntity> TryChangeLoginIdentifierAsync(T identity, string newLogin)
        {
            var user = await identityQuery.GetIdentityByEmail(newLogin);

            identityValidator.ValidateRegisterAsync(user);

            await UpdateUserLogin(user, newLogin);
            return user;
        }

        public async Task<IUserIdentityEntity> TryChangePasswordAsync(T identity, string oldPassword, string newPassword)
        {
            var user = await identityQuery.GetIdentityByEmail(identity.Email);

            string hashedOldPassword = passwordHasher.GetHash(oldPassword);
            identityValidator.ValidateLoginAsync(user, hashedOldPassword);

            await UpdateUserPassword(user, newPassword);
            return user;
        }

        private async Task UpdateUserLogin(T user, string newLogin)
        {
            user.Email = newLogin;
            await identityRepository.Update(user);
        }

        private async Task UpdateUserPassword(T user, string newPassword)
        {
            user.HashPassword = passwordHasher.GetHash(newPassword);
            await identityRepository.Update(user);
        }
    }
}
