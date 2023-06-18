using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.Authentification.Login;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.BLL.Services.Session.Tokens;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Register
{
    public class EmailEmployeeRegistrationService : IEmailEmployeeRegistrationService
    {
        private readonly IEmailSender emailSender;
        private readonly IRegistrationTokensService tokensService;
        private readonly IAuthorizationService<EmployeeIdentity, EmployeeProfile> authorizationService;
        private readonly UserManager<EmployeeIdentity> userManager;


        public EmailEmployeeRegistrationService(
            IEmailSender emailSender, 
            IRegistrationTokensService tokensService,
            IAuthorizationService<EmployeeIdentity, EmployeeProfile> authorizationService,
            UserManager<EmployeeIdentity> userManager)
        {
            this.emailSender = emailSender;
            this.tokensService = tokensService;
            this.authorizationService = authorizationService;
            this.userManager = userManager;
        }

        public async Task TrySendConfirmationTokenAsync(string email, string password)
        {
            if (userManager.GetUserByEmail(email) != null)
            {
                throw new AuthenticationException(AuthenticationException.WrongEmail);
            }

            var token = await tokensService.RegisterNewTokenAsync(email, password);
            await emailSender.SendEmailAsync(email, "Registration", GetConfirmationUrl(token.Id));
        }

        public async Task TryRegisterByTokenAsync(string tokenId, HttpContext httpContext)
        {
            var token = await TryGetTokenAsync(tokenId);
            var user = GetEmployeeIdentity(token);
            await userManager.AddNewUser(user);

            var addedUser = userManager.GetUserByEmail(user.Email);
            await authorizationService.SignInHttpContextAsync(httpContext, addedUser.Id);
        }

        private string GetConfirmationUrl(string tokenId)
        {
            return $"{tokenId}";
        }

        private Task<RegistrationToken> TryGetTokenAsync(string tokenId)
        {
            try
            {
                return tokensService.TryGetTokenAsync(tokenId);
            }
            catch (TokenValidationException e)
            {
                throw new AuthenticationException(e.Message);
            }
        }

        private EmployeeIdentity GetEmployeeIdentity(RegistrationToken token)
        {
            return new EmployeeIdentity()
            {
                Email = token.RelatedLogin,
                HashPassword = token.PasswordHash,
                Profile = new EmployeeProfile()
            };
        }
    }
}
