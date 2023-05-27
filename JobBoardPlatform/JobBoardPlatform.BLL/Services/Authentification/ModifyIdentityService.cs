using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class ModifyIdentityService<T> : IModifyIdentityService<T>
        where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> identityRepository;
        private readonly IIdentityValidator<T> validateIdentity;
        private readonly IPasswordHasher passwordHasher;


        public ModifyIdentityService(IRepository<T> repository)
        {
            this.identityRepository = repository;
            this.validateIdentity = new IdentityValidator<T>();
            this.passwordHasher = new MD5Hasher();
        }

        public async Task<AuthentificationResult> TryChangeLoginIdentifier(T identity, string newLogin)
        {
            var user = await GetUserByEmailAsync(newLogin);
            var validate = validateIdentity.ValidateRegister(user);

            if (validate.IsError)
            {
                return validate;
            }

            var userIdentity = await identityRepository.Get(identity.Id);
            userIdentity!.Email = newLogin;
            await identityRepository.Update(userIdentity);

            var success = AuthentificationResult.Success;
            success.FoundRecord = userIdentity;

            return success;
        }

        public async Task<AuthentificationResult> ChangePassword(T identity, string oldPassword, string newPassword)
        {
            string hashedOldPassword = passwordHasher.GetHash(oldPassword);

            var user = await identityRepository.Get(identity.Id);
            var validate = validateIdentity.ValidateLogin(user, hashedOldPassword);

            if (validate.IsError)
            {
                return validate;
            }

            user!.HashPassword = passwordHasher.GetHash(newPassword);
            await identityRepository.Update(user);

            var success = AuthentificationResult.Success;
            success.FoundRecord = user;

            return success;
        }

        /// <summary>
        /// Utility helper function
        /// </summary>
        private async Task<T?> GetUserByEmailAsync(string email)
        {
            var userSet = await identityRepository.GetAllSet();
            var user = await userSet.FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }
    }
}
