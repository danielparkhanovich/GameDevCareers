using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Login
{
    public class LoginService<TEntity, TProfile> : ILoginService<TEntity, TProfile>
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly UserManager<TEntity> userManager;
        private readonly IAuthorizationService<TEntity, TProfile> authorizationService;
        private readonly IPasswordHasher passwordHasher;


        public LoginService(
            UserManager<TEntity> userManager,
            IAuthorizationService<TEntity, TProfile> authorizationService,
            IPasswordHasher passwordHasher)
        {
            this.userManager = userManager;
            this.authorizationService = authorizationService;
            this.passwordHasher = passwordHasher;
        }

        public async Task<TEntity> ForceLoginAsync(string email, HttpContext httpContext)
        {
            var user = await userManager.GetUserByEmail(email);
            ValidateEmail(user);

            await authorizationService.SignInHttpContextAsync(httpContext, user.Id);
            return user;
        }

        public async Task<TEntity> TryLoginAsync(string email, string enteredPassword, HttpContext httpContext)
        {
            var user = await userManager.GetUserByEmail(email);
            ValidateEmail(user);
            ValidateEnteredPassword(user.HashPassword, enteredPassword);

            await authorizationService.SignInHttpContextAsync(httpContext, user.Id);
            return user;
        }

        private void ValidateEmail(TEntity user)
        {
            if (user == null)
            {
                throw new AuthenticationException(AuthenticationException.EmailNotFound);
            }
        }

        private void ValidateEnteredPassword(string originalHash, string enteredPassword)
        {
            string passwordHash = passwordHasher.GetHash(enteredPassword);
            if (passwordHash != originalHash)
            {
                throw new AuthenticationException(AuthenticationException.WrongPassword);
            }
        }
    }
}
