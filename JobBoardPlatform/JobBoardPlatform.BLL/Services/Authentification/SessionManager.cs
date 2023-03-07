using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class SessionManager : ISessionManager
    {
        private readonly HttpContext httpContext;


        public SessionManager(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public async Task SignInHttpContextAsync(ICredentialEntity credentials)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, credentials.Email),
                // new Claim("Other properties", "Example role")
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
