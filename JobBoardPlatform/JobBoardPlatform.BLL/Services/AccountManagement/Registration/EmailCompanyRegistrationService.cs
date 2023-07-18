using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Register
{
    public class EmailCompanyRegistrationService : IEmailCompanyRegistrationService
    {
        private readonly IEmailSender emailSender;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IRegistrationTokensService tokensService;
        private readonly IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService;
        private readonly UserManager<CompanyIdentity> userManager;


        public EmailCompanyRegistrationService(
            IEmailSender emailSender,
            IPasswordGenerator passwordGenerator,
            IRegistrationTokensService tokensService,
            IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService,
            UserManager<CompanyIdentity> userManager)
        {
            this.emailSender = emailSender;
            this.passwordGenerator = passwordGenerator;
            this.tokensService = tokensService;
            this.authorizationService = authorizationService;
            this.userManager = userManager;
        }

        public async Task TrySendConfirmationTokenAndPasswordAsync(string email)
        {
            if (userManager.GetUserByEmailAsync(email) != null)
            {
                throw new AuthenticationException(AuthenticationException.EmailAlreadyRegistered);
            }

            string password = passwordGenerator.GeneratePassword();
            var token = await tokensService.RegisterNewTokenAsync(email, password);
            await emailSender.SendEmailAsync(email, "Registration-company", GetConfirmationUrl(token.Id) + " " + password);
        }

        public async Task TryRegisterByTokenAsync(string tokenId, HttpContext httpContext)
        {
            var token = await TryGetTokenAsync(tokenId);
            var user = GetCompanyIdentity(token.RelatedLogin, token.PasswordHash);
            await userManager.AddNewUser(user);

            var addedUser = userManager.GetUserByEmailAsync(user.Email);
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

        private CompanyIdentity GetCompanyIdentity(string email, string passwordHash)
        {
            return new CompanyIdentity()
            {
                Email = email,
                HashPassword = passwordHash,
                Profile = new CompanyProfile()
            };
        }
    }
}
