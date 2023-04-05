using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Common;
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
            string idStr = httpContext.User.FindFirst(UserSessionProperties.NameIdentifier)!.Value;

            int id = int.Parse(idStr);

            var profileAdapter = UserProfileAdapterFactory.CreateProfileAdapter(profile);

            await authorizationService.SignOutHttpContextAsync();
            var updated = new AuthorizationData()
            {
                Id = id,
                ProfileId = profile.Id,
                DisplayName = profileAdapter.DisplayName,
                DisplayImageUrl = profileAdapter.DisplayProfileImageUrl,
                Role = profileAdapter.UserRole
            };
            await authorizationService.SignInHttpContextAsync(updated);
        }
    }
}
