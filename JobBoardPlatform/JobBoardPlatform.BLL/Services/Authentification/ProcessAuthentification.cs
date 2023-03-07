using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class ProcessAuthentification<T> : IProcessAuthentification<T> 
        where T : class, ICredentialEntity
    {
        private readonly IRepository<T> repository;
        private readonly ISessionManager sessionManager;
        private readonly IValidateCredentials validateCredentials;
        private readonly IPasswordHasher passwordHasher;


        public ProcessAuthentification(IRepository<T> repository, HttpContext httpContext)
        {
            this.repository = repository;
            this.sessionManager = new SessionManager(httpContext);
            this.validateCredentials = new ValidateCredentials<T>(repository);
            this.passwordHasher = new PasswordHasher();
        }

        public async Task<AuthorizationResult> TryRegisterAsync(T credentials)
        {
            var validate = await validateCredentials.ValidateRegisterAsync(credentials.Email);
            if (validate.IsError)
            {
                return validate;
            }

            // hash raw password
            string hashedPassword = passwordHasher.HashPassword(credentials.HashPassword);
            credentials.HashPassword = hashedPassword;

            await repository.Add(credentials);

            await sessionManager.SignInHttpContextAsync(credentials);

            return AuthorizationResult.Success;
        }

        public async Task<AuthorizationResult> TryLoginAsync(T credentials)
        {
            string hashedPassword = passwordHasher.HashPassword(credentials.HashPassword);

            var validate = await validateCredentials.ValidateLoginAsync(credentials.Email, hashedPassword);
            if (validate.IsError)
            {
                return validate;
            }

            await sessionManager.SignInHttpContextAsync(credentials);

            return AuthorizationResult.Success;
        }
    }
}
