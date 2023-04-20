using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpContext httpContext;


        public AuthorizationService(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public async Task SignInHttpContextAsync(AuthorizationData data)
        {
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

        public async Task SignOutHttpContextAsync()
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
    }
}
