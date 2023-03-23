using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Common;
using JobBoardPlatform.BLL.Services.Session.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
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
            int id = int.Parse(httpContext.User.FindFirst("NameIdentifier").Value);

            var profileAdapter = UserProfileAdapterFactory.CreateProfileAdapter(profile);

            await authorizationService.SignOutHttpContextAsync();
            var updated = new AuthorizationData()
            {
                Id = id,
                DisplayName = profileAdapter.DisplayName,
                DisplayImageUrl = profileAdapter.DisplayProfileImageUrl,
                Role = profileAdapter.UserRole
            };
            await authorizationService.SignInHttpContextAsync(updated);
        }
    }
}
