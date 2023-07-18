using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Password.Tokens;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password
{
    public class RestorePasswordService<TEntity, TProfile> : IResetPasswordService<TEntity, TProfile>
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly IEmailSender emailSender;
        private readonly IRestorePasswordTokensService tokensService;
        private readonly IConfirmationLinkFactory linkFactory;
        private readonly UserManager<TEntity> userManager;
        private readonly IModifyIdentityService<TEntity> modifyIdentity;
        private readonly IAuthorizationService<TEntity, TProfile> authorizationService;


        public RestorePasswordService(
            IEmailSender emailSender,
            IRestorePasswordTokensService tokensService,
            IConfirmationLinkFactory linkFactory,
            UserManager<TEntity> userManager,
            IModifyIdentityService<TEntity> modifyIdentity,
            IAuthorizationService<TEntity, TProfile> authorizationService)
        {
            this.emailSender = emailSender;
            this.tokensService = tokensService;
            this.linkFactory = linkFactory;
            this.userManager = userManager;
            this.modifyIdentity = modifyIdentity;
            this.authorizationService = authorizationService;
        }

        public async Task TrySendResetPasswordTokenAsync(string email)
        {
            if (await userManager.GetUserByEmailAsync(email) == null)
            {
                throw new AuthenticationException(AuthenticationException.EmailNotFound);
            }

            var token = await tokensService.RegisterNewTokenAsync(email);
            await emailSender.SendEmailAsync(email, "Restore password", GetConfirmationUrl(token.Id));
        }

        public async Task TryChangePasswordByTokenAsync(string tokenId, string newPassword, HttpContext httpContext)
        {
            var token = await TryGetTokenAsync(tokenId);
            var user = await modifyIdentity.ForceChangePasswordAsync(token.RelatedLogin, newPassword);
            await authorizationService.SignInHttpContextAsync(httpContext, user.Id);
        }

        private string GetConfirmationUrl(string tokenId)
        {
            return linkFactory.CreateConfirmationLink(tokenId);
        }

        private async Task<RestorePasswordToken> TryGetTokenAsync(string tokenId)
        {
            try
            {
                return await tokensService.TryGetTokenAsync(tokenId);
            }
            catch (TokenValidationException e)
            {
                throw new AuthenticationException(e.Message);
            }
        }
    }
}
