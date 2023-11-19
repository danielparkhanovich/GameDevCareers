using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Register
{
    public class EmailCompanyPublishOfferAndRegistrationService
    {
        private readonly IRegistrationEmailSender emailSender;
        private readonly IRegistrationTokensService registrationTokensService;
        private readonly ConfirmationTokensService confirmationTokensService;
        private readonly DataTokensService<CompanyProfileAndNewOfferData> dataTokensService;
        private readonly IConfirmationLinkFactory linkFactory;
        private readonly IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService;
        private readonly UserManager<CompanyIdentity> userManager;
        private readonly IOfferManager offersManager;


        public EmailCompanyPublishOfferAndRegistrationService(
            IRegistrationEmailSender emailSender,
            IRegistrationTokensService registrationTokensService,
            ConfirmationTokensService confirmationTokensService,
            DataTokensService<CompanyProfileAndNewOfferData> dataTokensService,
            IConfirmationLinkFactory linkFactory,
            IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService,
            UserManager<CompanyIdentity> userManager,
            IOfferManager offersManager)
        {
            this.emailSender = emailSender;
            this.registrationTokensService = registrationTokensService;
            this.confirmationTokensService = confirmationTokensService;
            this.dataTokensService = dataTokensService;
            this.linkFactory = linkFactory;
            this.authorizationService = authorizationService;
            this.userManager = userManager;
            this.offersManager = offersManager;
        }

        public async Task TrySendConfirmationTokenAsync(string email, string password, string formDataTokenId)
        {
            if (await userManager.IsExistsWithEmailAsync(email))
            {
                throw new AuthenticationException(AuthenticationException.EmailAlreadyRegistered);
            }

            var token = await registrationTokensService.RegisterNewTokenAsync(email, password);

            await confirmationTokensService.RegisterNewTokenAsync((token.Id, formDataTokenId));
            await emailSender.SendEmailAsync(email, GetConfirmationUrl(token.Id));
        }

        public async Task TryRegisterByTokenAsync(string tokenId, HttpContext httpContext)
        {
            var registrationToken = await TryGetRegistrationTokenAsync(tokenId);
            if (await userManager.GetWithEmailAsync(registrationToken.RelatedLogin) != null)
            {
                return;
            }

            var dataToken = await GetDataToken(tokenId);

            await RegisterUser(registrationToken, dataToken.Value.CompanyProfileData);

            var addedUser = await userManager.GetWithEmailAsync(registrationToken.RelatedLogin);
            await authorizationService.SignInHttpContextAsync(httpContext, addedUser.Id);

            await CreateOffer(addedUser, dataToken.Value.OfferData);
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

        private async Task<DataToken<CompanyProfileAndNewOfferData>> GetDataToken(string tokenId)
        {
            var confirmationToken = await confirmationTokensService.TryGetTokenAsync(tokenId);
            var dataToken = await dataTokensService.TryGetTokenAsync(confirmationToken.TokenToConfirmId);
            return dataToken;
        }

        private Task RegisterUser(RegistrationToken token, CompanyProfileData profileData)
        {
            var user = GetCompanyIdentity(token.RelatedLogin, token.PasswordHash);
            user.Profile = GetCompanyProfile(profileData);
            return userManager.AddAsync(user);
        }

        private Task CreateOffer(CompanyIdentity company, OfferData offerData)
        {
            return offersManager.AddAsync(company.ProfileId, offerData);
        }

        private CompanyProfile GetCompanyProfile(CompanyProfileData profileData)
        {
            var companyProfile = new CompanyProfile();
            var mapper = new CompanyDataToCompanyProfileMapper();
            mapper.Map(profileData, companyProfile);
            return companyProfile;
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
