using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Policies.IdentityProviders;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization
{
    public class EmployeeIdentityServiceWithProvider : IIdentityServiceWithProvider<EmployeeIdentity>
    {
        /// <summary>
        /// Initially password is not set
        /// </summary>
        private static string PasswordNotSet = "N/A";

        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IdentityQueryExecutor<EmployeeIdentity> queryExecutor;


        public EmployeeIdentityServiceWithProvider(
            IRepository<EmployeeIdentity> identityRepository, 
            IRepository<EmployeeProfile> profileRepository,
            IdentityQueryExecutor<EmployeeIdentity> queryExecutor)
        {
            this.identityRepository = identityRepository;
            this.profileRepository = profileRepository;
            this.queryExecutor = queryExecutor;
        }

        public Task<EmployeeIdentity> TryLoginOrRegisterAsync(
            HttpContext httpContext, IIdentityProviderClaimKeys claimKeys)
        {
            var authentification = new AuthentificationServiceWithIdentityProvider<EmployeeIdentity>(
                identityRepository);

            var userService = new IdentityService<EmployeeIdentity, EmployeeProfile>(
                httpContext, authentification, profileRepository);

            var user = GetProvidedIdentity(httpContext, claimKeys);
            return HandleIdentityAuthorizationWithDefaultService(user, userService);
        }

        private EmployeeIdentity GetProvidedIdentity(HttpContext httpContext, IIdentityProviderClaimKeys claimKeys)
        {
            string userEmail = TryGetClaimValue(httpContext.User, claimKeys.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new AuthentificationException(AuthentificationException.EmailWrong);
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

        private async Task<EmployeeIdentity> HandleIdentityAuthorizationWithDefaultService(
            EmployeeIdentity user, IdentityService<EmployeeIdentity, EmployeeProfile> userService)
        {
            if (await queryExecutor.GetIdentityByEmail(user.Email) == null)
            {
                return await userService.TryRegisterAsync(user);
            }
            else
            {
                return await userService.TryLoginAsync(user);
            }
        }
    }
}
