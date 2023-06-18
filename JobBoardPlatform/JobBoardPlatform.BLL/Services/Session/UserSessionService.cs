using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Login;
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Session
{
    public class UserSessionService<TEntity, TProfile> : IUserSessionService<TEntity, TProfile>
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly IAuthorizationService<TEntity, TProfile> authorizationService;


        public UserSessionService(IAuthorizationService<TEntity, TProfile> authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public async Task UpdateSessionStateAsync(HttpContext httpContext)
        {
            int id = UserSessionUtils.GetIdentityId(httpContext.User);
            await authorizationService.SignOutHttpContextAsync(httpContext);
            await authorizationService.SignInHttpContextAsync(httpContext, id);
        }
    }
}
