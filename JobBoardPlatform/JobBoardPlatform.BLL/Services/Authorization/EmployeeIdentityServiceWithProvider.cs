using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Common;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization
{
    public class EmployeeIdentityServiceWithProvider : IIdentityServiceWithProvider<EmployeeIdentity>
    {
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
            HttpContext httpContext, string userImageKey)
        {
            var authentification = new AuthentificationServiceWithIdentityProvider<EmployeeIdentity>(
                identityRepository);

            var userService = new IdentityService<EmployeeIdentity, EmployeeProfile>
                (httpContext, authentification, profileRepository);

            var user = GetProvidedIdentity(httpContext, userImageKey);
            return HandleIdentityCheckWithDefaultService(user, userService);
        }

        private EmployeeIdentity GetProvidedIdentity(HttpContext httpContext, string userImageKey)
        {
            var user = httpContext.User;
            string? userEmail = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string? userName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            string? userSurname = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            string? imageUrl = user.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new AuthentificationException(AuthentificationException.EmailWrong);
            }

            return new EmployeeIdentity()
            {
                Email = userEmail!,
                HashPassword = AuthentificationValues.PasswordNotSet,
                Profile = new EmployeeProfile()
                {
                    Name = userName ?? string.Empty,
                    Surname = userSurname ?? string.Empty,
                    ProfileImageUrl = imageUrl ?? string.Empty
                }
            };
        }

        private async Task<EmployeeIdentity> HandleIdentityCheckWithDefaultService(
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
