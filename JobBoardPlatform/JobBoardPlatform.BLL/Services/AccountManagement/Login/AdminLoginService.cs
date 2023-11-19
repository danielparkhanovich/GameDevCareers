using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Admin;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Login
{
    public class AdminLoginService : ILoginService<AdminIdentity>
    {
        private readonly UserManager<AdminIdentity> userManager;
        private readonly IPasswordHasher passwordHasher;
        private readonly AuthorizationServiceCore authorization;


        public AdminLoginService(
            UserManager<AdminIdentity> userManager,
            IPasswordHasher passwordHasher,
            AuthorizationServiceCore authorization)
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.authorization = authorization;
        }

        public async Task<AdminIdentity> TryLoginAsync(string email, string enteredPassword, HttpContext httpContext)
        {
            var user = await userManager.GetWithEmailAsync(email);
            ValidateEmail(user);
            ValidateEnteredPassword(user.HashPassword, enteredPassword);

            var data = GetAuthorizationData(user);
            await authorization.SignInHttpContextAsync(httpContext, data);
            return user;
        }

        private void ValidateEmail(AdminIdentity user)
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

        private AuthorizationData GetAuthorizationData(AdminIdentity user)
        {
            return new AuthorizationData() 
            { 
                Id = user.Id,
                DisplayName = user.Email,
                Role = UserRoles.Admin,
            };
        }
    }
}
