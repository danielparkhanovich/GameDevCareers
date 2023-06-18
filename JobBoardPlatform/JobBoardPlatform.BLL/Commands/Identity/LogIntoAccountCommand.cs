using JobBoardPlatform.BLL.Services.Authentification.Login;
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    public class LogIntoAccountCommand<TIdentity, TProfile> : ICommand
        where TIdentity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly HttpContext httpContext;
        private readonly AuthorizationService<TIdentity, TProfile> authorizationService;
        private readonly int idLogInto;


        public LogIntoAccountCommand(
            HttpContext httpContext,
            AuthorizationService<TIdentity, TProfile> authorizationService,
            int idLogInto)
        {
            this.httpContext = httpContext;
            this.authorizationService = authorizationService;
            this.idLogInto = idLogInto;
        }

        public async Task Execute()
        {
            await SignOut();
            await SignIntoAccount();
        }

        private Task SignOut()
        {
            return authorizationService.SignOutHttpContextAsync(httpContext);
        }

        private async Task SignIntoAccount()
        {
            await authorizationService.SignInHttpContextAsync(httpContext, idLogInto);
        }
    }
}
