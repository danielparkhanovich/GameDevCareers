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
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, data.NameIdentifier),
                new Claim(ClaimTypes.Name, data.DisplayName),
                new Claim(ClaimTypes.Role, data.Role)
            };

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
    }
}
