using JobBoardPlatform.BLL.Common.ProfileAdapter;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Session.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Session
{
    public class UserSessionService<T> : IUserSessionService<T>
        where T: class, IUserProfileEntity
    {
        private readonly HttpContext httpContext;
        private readonly IAuthorizationService authorizationService;


        public UserSessionService(HttpContext httpContext)
        {
            this.httpContext = httpContext;
            this.authorizationService = new AuthorizationService(httpContext);
        }

        public async Task UpdateSessionStateAsync(T profile)
        {
            int id = UserSessionUtils.GetIdentityId(httpContext.User);

            await authorizationService.SignOutHttpContextAsync();
            var updated = new AuthorizationData(id, string.Empty, profile);
            await authorizationService.SignInHttpContextAsync(updated);
        }
    }
}
