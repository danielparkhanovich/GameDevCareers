using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class ProcessAutorization<T> : IProcessAutorization<T> 
        where T : class, ICredentialEntity
    {
        private readonly IRepository<T> repository;
        private readonly IValidateCredentials validateCredentials;
        private readonly IPasswordHasher passwordHasher;


        public ProcessAutorization(IRepository<T> repository)
        {
            this.repository = repository;
            validateCredentials = new ValidateCredentials<T>(repository);
            passwordHasher = new PasswordHasher();
        }

        public async Task<AuthorizationResult> TryRegister(T credentials)
        {
            var validate = await validateCredentials.ValidateRegistration(credentials.Email);
            if (validate.IsError)
            {
                return validate;
            }

            var result = new AuthorizationResult();

            string hashedPassword = passwordHasher.HashPassword(credentials.HashPassword);
            credentials.HashPassword = hashedPassword;

            await repository.Add(credentials);

            return result;
        }

        public async Task<AuthorizationResult> TryLogin(T credentials)
        {
            string hashedPassword = passwordHasher.HashPassword(credentials.HashPassword);

            var validate = await validateCredentials.ValidateLogin(credentials.Email, hashedPassword);
            if (validate.IsError)
            {
                return validate;
            }

            return validate;
        }
    }
}
