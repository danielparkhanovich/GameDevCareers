using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class AuthentificationService<T> : IAuthentificationService<T> 
        where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> identityRepository;
        private readonly IIdentityValidator<T> validateIdentity;
        private readonly IPasswordHasher passwordHasher;


        public AuthentificationService(IRepository<T> repository)
        {
            this.identityRepository = repository;
            this.validateIdentity = new IdentityValidator<T>();
            this.passwordHasher = new MD5Hasher();
        }

        public async Task<AuthentificationResult> TryRegisterAsync(T identity)
        {
            var user = await GetUserByEmailAsync(identity.Email);
            var validate = validateIdentity.ValidateRegister(user);

            if (validate.IsError)
            {
                return validate;
            }

            // hash raw password
            string hashedPassword = passwordHasher.GetHash(identity.HashPassword);
            identity.HashPassword = hashedPassword;

            var success = AuthentificationResult.Success;

            var addNewUserCommand = new AddNewUserCommand<T>(identityRepository, identity);
            await addNewUserCommand.Execute();

            success.FoundRecord = addNewUserCommand.AddedRecord;

            return success;
        }

        public async Task<AuthentificationResult> TryLoginAsync(T identity)
        {
            string hashedPassword = passwordHasher.GetHash(identity.HashPassword);

            var user = await GetUserByEmailAsync(identity.Email);
            var validate = validateIdentity.ValidateLogin(user, hashedPassword);

            if (validate.IsError)
            {
                return validate;
            }

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
