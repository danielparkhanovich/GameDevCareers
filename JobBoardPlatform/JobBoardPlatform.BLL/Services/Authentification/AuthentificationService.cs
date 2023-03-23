using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class AuthentificationService<T> : IAuthentificationService<T> 
        where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> identityRepository;
        private readonly ICredentialsValidator<T> validateCredentials;
        private readonly IPasswordHasher passwordHasher;


        public AuthentificationService(IRepository<T> repository)
        {
            this.identityRepository = repository;
            this.validateCredentials = new CredentialsValidator<T>();
            this.passwordHasher = new PasswordHasher();
        }

        public async Task<AuthentificationResult> TryRegisterAsync(T credentials)
        {
            var user = await GetUserByEmailAsync(credentials.Email);
            var validate = validateCredentials.ValidateRegister(user);

            if (validate.IsError)
            {
                return validate;
            }

            // hash raw password
            string hashedPassword = passwordHasher.HashPassword(credentials.HashPassword);
            credentials.HashPassword = hashedPassword;

            var success = AuthentificationResult.Success;

            success.FoundRecord = await identityRepository.Add(credentials);

            return success;
        }

        public async Task<AuthentificationResult> TryLoginAsync(T credentials)
        {
            string hashedPassword = passwordHasher.HashPassword(credentials.HashPassword);

            var user = await GetUserByEmailAsync(credentials.Email);
            var validate = validateCredentials.ValidateLogin(user, hashedPassword);

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
