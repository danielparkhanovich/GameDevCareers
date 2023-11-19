using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authentification.Login
{
    public class AuthorizationServiceCore
    {
        public async Task SignInHttpContextAsync(HttpContext httpContext, AuthorizationData data)
        {
            if (UserSessionUtils.IsLoggedIn(httpContext.User))
            {
                await SignOutHttpContextAsync(httpContext);
            }

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
    }
}
