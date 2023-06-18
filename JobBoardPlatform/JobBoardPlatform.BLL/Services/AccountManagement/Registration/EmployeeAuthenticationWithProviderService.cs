using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.IdentityProviders;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Employee;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authentification.Register
{
    public class EmployeeAuthenticationWithProviderService : IAuthenticationWithProviderService<EmployeeIdentity>
    {
        /// <summary>
        /// Initially password is not set
        /// </summary>
        private static string PasswordNotSet = "N/A";

        private readonly IRegistrationService<EmployeeIdentity> registrationService;
        private readonly ILoginService<EmployeeIdentity, EmployeeProfile> loginService;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly UserManager<EmployeeIdentity> userManager;


        public EmployeeAuthenticationWithProviderService(
            IRegistrationService<EmployeeIdentity> registrationService,
            ILoginService<EmployeeIdentity, EmployeeProfile> loginService,
            IPasswordGenerator passwordGenerator,
            UserManager<EmployeeIdentity> userManager)
        {
            this.registrationService = registrationService;
            this.loginService = loginService;
            this.passwordGenerator = passwordGenerator;
            this.userManager = userManager;
        }

        public Task<EmployeeIdentity> TryLoginOrRegisterAsync(
            HttpContext httpContext, IIdentityProviderClaimKeys claimKeys)
        {
            var user = GetProvidedIdentity(httpContext, claimKeys);
            return HandleAuthenticationWithDefaultServices(user, httpContext);
        }

        private EmployeeIdentity GetProvidedIdentity(HttpContext httpContext, IIdentityProviderClaimKeys claimKeys)
        {
            string userEmail = TryGetClaimValue(httpContext.User, claimKeys.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new AuthenticationException(AuthenticationException.WrongEmail);
            }

            return new EmployeeIdentity()
            {
                Email = userEmail,
                HashPassword = PasswordNotSet,
                Profile = GetFilledEmployeeProfile(httpContext, claimKeys),
            };
        }

        private EmployeeProfile GetFilledEmployeeProfile(HttpContext httpContext, IIdentityProviderClaimKeys claimKeys)
        {
            return new EmployeeProfile()
            {
                Name = TryGetClaimValue(httpContext.User, claimKeys.Name),
                Surname = TryGetClaimValue(httpContext.User, claimKeys.Surname),
                ProfileImageUrl = TryGetClaimValue(httpContext.User, claimKeys.UserImageUrl),
                City = TryGetLocation(httpContext.User, claimKeys.Location).Item1,
                Country = TryGetLocation(httpContext.User, claimKeys.Location).Item2,
                Description = TryGetClaimValue(httpContext.User, claimKeys.Bio)
            };
        }

        private string TryGetClaimValue(ClaimsPrincipal user, string? claimType)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                return string.Empty;
            }
            return user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
        }

        /// <returns>(city, country)</returns>
        private (string, string) TryGetLocation(ClaimsPrincipal user, string? claimType)
        {
            string location = TryGetClaimValue(user, claimType);
            if (string.IsNullOrEmpty(location) || location.Split(" ").Length < 2)
            {
                return (string.Empty, string.Empty);
            }
            return (location.Split(" ")[0], location.Split(" ")[1]);
        }

        private async Task<EmployeeIdentity> HandleAuthenticationWithDefaultServices(
            EmployeeIdentity user, HttpContext httpContext)
        {
            if (await userManager.GetUserByEmail(user.Email) == null)
            {
                string password = passwordGenerator.GeneratePassword();
                await registrationService.TryRegisterAsync(user.Email, password);
            }
            return await loginService.ForceLoginAsync(user.Email, httpContext);
        }
    }
}
