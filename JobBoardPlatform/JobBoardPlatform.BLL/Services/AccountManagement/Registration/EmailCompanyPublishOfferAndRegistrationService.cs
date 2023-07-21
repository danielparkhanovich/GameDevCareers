using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Commands.Profile;
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
    public class EmailCompanyPublishOfferAndRegistrationService
    {
        private readonly IEmailSender emailSender;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IRegistrationTokensService registrationTokensService;
        private readonly ConfirmationTokensService confirmationTokensService;
        private readonly DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService;
        private readonly IConfirmationLinkFactory linkFactory;
        private readonly IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService;
        private readonly UserManager<CompanyIdentity> userManager;


        public EmailCompanyPublishOfferAndRegistrationService(
            IEmailSender emailSender,
            IPasswordGenerator passwordGenerator,
            IRegistrationTokensService registrationTokensService,
            ConfirmationTokensService confirmationTokensService,
            DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService,
            IConfirmationLinkFactory linkFactory,
            IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService,
            UserManager<CompanyIdentity> userManager)
        {
            this.emailSender = emailSender;
            this.passwordGenerator = passwordGenerator;
            this.registrationTokensService = registrationTokensService;
            this.confirmationTokensService = confirmationTokensService;
            this.dataTokensService = dataTokensService;
            this.linkFactory = linkFactory;
            this.authorizationService = authorizationService;
            this.userManager = userManager;
        }

        public async Task TrySendConfirmationTokenAndPasswordAsync(string email, string formDataTokenId)
        {
            if (userManager.GetUserByEmailAsync(email) != null)
            {
                throw new AuthenticationException(AuthenticationException.EmailAlreadyRegistered);
            }

            string password = passwordGenerator.GeneratePassword();
            var token = await registrationTokensService.RegisterNewTokenAsync(email, password);

            await confirmationTokensService.RegisterNewTokenAsync((token.Id, formDataTokenId));
            await emailSender.SendEmailAsync(email, "Registration-company", GetConfirmationUrl(token.Id) + " " + password);
        }

        public async Task TryRegisterByTokenAsync(string tokenId, HttpContext httpContext)
        {
            var token = await TryGetRegistrationTokenAsync(tokenId);
            var dataToken = await GetDataToken(tokenId);
            dataToken.IsConfirmed = true;

            var user = GetCompanyIdentity(token.RelatedLogin, token.PasswordHash);
            await userManager.AddNewUser(user);

            var addedUser = await userManager.GetUserByEmailAsync(token.RelatedLogin);
            /*var updateProfileCommand = new UpdateCompanyProfileCommand(addedUser.ProfileId,
                dataToken.Value.CompanyProfileData,
                profileRepository,
                imageStorage);
            await updateProfileCommand.Execute();*/

            // var addedUser = userManager.GetUserByEmailAsync(user.Email);
            await authorizationService.SignInHttpContextAsync(httpContext, addedUser.Id);
        }

        private string GetConfirmationUrl(string tokenId)
        {
            return linkFactory.CreateConfirmationLink(tokenId);
        }

        private Task<RegistrationToken> TryGetRegistrationTokenAsync(string tokenId)
        {
            try
            {
                return registrationTokensService.TryGetTokenAsync(tokenId);
            }
            catch (TokenValidationException e)
            {
                throw new AuthenticationException(e.Message);
            }
        }

        private async Task<DataToken<ICompanyProfileAndNewOfferData>> GetDataToken(string tokenId)
        {
            var confirmationToken = await confirmationTokensService.TryGetTokenAsync(tokenId);
            return await dataTokensService.TryGetTokenAsync(confirmationToken.TokenToConfirmId);
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
