using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authorization
{
    public class IdentityService<TIdentity, TProfile> : IAuthentificationService<TIdentity>
        where TIdentity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly IRepository<TProfile> profileRepository;
        private readonly IAuthentificationService<TIdentity> authentificationService;
        private readonly IAuthorizationService authorizationService;


        public IdentityService(
            HttpContext httpContext,
            IAuthentificationService<TIdentity> authentificationService, 
            IRepository<TProfile> profileRepository)
        {
            this.profileRepository = profileRepository;
            this.authentificationService = authentificationService;
            this.authorizationService = new AuthorizationService(httpContext);
        }

        public async Task<TIdentity> TryLoginAsync(TIdentity identity)
        {
            var authentificationFunc = authentificationService.TryLoginAsync;
            return await TryAuthorize(authentificationFunc, identity);
        }

        public async Task<TIdentity> TryRegisterAsync(TIdentity identity)
        {
            var authentificationFunc = authentificationService.TryRegisterAsync;
            return await TryAuthorize(authentificationFunc, identity);
        }

        private async Task<TIdentity> TryAuthorize(Func<TIdentity, Task<TIdentity>> authentificationFunc,
            TIdentity identity)
        {
            var user = await authentificationFunc(identity);
            var userData = await GetAuthorizationDataAsync(user);
            await authorizationService.SignInHttpContextAsync(userData);
            return user;
        }

        private async Task<AuthorizationData> GetAuthorizationDataAsync(TIdentity user)
        {
            var profileRecord = await profileRepository.Get(user.ProfileId);
            return new AuthorizationData(user.Id, user.Email, profileRecord);
        }
    }
}
