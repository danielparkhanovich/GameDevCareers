using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authentification.Login
{
    public class AuthorizationService<TEntity, TProfile> : IAuthorizationService<TEntity, TProfile>
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly IRepository<TEntity> userRepository;
        private readonly IRepository<TProfile> profilesRepository;


        public AuthorizationService(
            IRepository<TEntity> userRepository,
            IRepository<TProfile> profilesRepository)
        {
            this.userRepository = userRepository;
            this.profilesRepository = profilesRepository;
        }

        public async Task SignInHttpContextAsync(HttpContext httpContext, int userId)
        {
            if (UserSessionUtils.IsLoggedIn(httpContext.User))
            {
                await SignOutHttpContextAsync(httpContext);
            }

            var data = await GetAuthorizationDataAsync(userId);

            List<Claim> claims = new List<Claim>();
            claims.AddRange(GetSpecificProperties(data));
            claims.AddRange(GetPersonalizationProperties(data));

            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);
        }

        public async Task SignOutHttpContextAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private List<Claim> GetSpecificProperties(AuthorizationData data)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
                new Claim(ClaimTypes.Name, data.DisplayName),
                new Claim(ClaimTypes.Role, data.Role)
            };
        }

        private List<Claim> GetPersonalizationProperties(AuthorizationData data)
        {
            return new List<Claim>()
            {
                new Claim(UserSessionProperties.NameIdentifier, data.Id.ToString()),
                new Claim(UserSessionProperties.ProfileIdentifier, data.ProfileId.ToString()),
                new Claim(UserSessionProperties.DisplayName, data.DisplayName),
                new Claim(UserSessionProperties.DisplayImageUrl, data.DisplayImageUrl),
                new Claim(UserSessionProperties.Role, data.Role)
            };
        }

        private async Task<AuthorizationData> GetAuthorizationDataAsync(int userId)
        {
            var user = await userRepository.Get(userId);
            var profileRecord = await profilesRepository.Get(user.ProfileId);
            return new AuthorizationData(user.Id, user.Email, profileRecord);
        }
    }
}
