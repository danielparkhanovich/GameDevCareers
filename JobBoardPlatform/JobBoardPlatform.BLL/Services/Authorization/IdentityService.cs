using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Common;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
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


        public IdentityService(HttpContext httpContext, 
            IRepository<TIdentity> userRepository, IRepository<TProfile> profileRepository)
        {
            this.profileRepository = profileRepository;
            this.authentificationService = new AuthentificationService<TIdentity>(userRepository);
            this.authorizationService = new AuthorizationService(httpContext);
        }

        public async Task<AuthentificationResult> TryLoginAsync(TIdentity Identitys)
        {
            var authentificationFunc = authentificationService.TryLoginAsync;

            return await TryAuthorize(authentificationFunc, Identitys);
        }

        public async Task<AuthentificationResult> TryRegisterAsync(TIdentity Identitys)
        {
            var authentificationFunc = authentificationService.TryRegisterAsync;

            return await TryAuthorize(authentificationFunc, Identitys);
        }

        private async Task<AuthentificationResult> TryAuthorize(Func<TIdentity, Task<AuthentificationResult>> authentificationFunc,
            TIdentity Identitys)
        {
            var authentification = await authentificationFunc(Identitys);
            if (authentification.IsError)
            {
                return authentification;
            }

            var userData = await GetAuthorizationDataAsync(authentification.FoundRecord!);

            await authorizationService.SignInHttpContextAsync(userData);

            return AuthentificationResult.Success;
        }

        private async Task<AuthorizationData> GetAuthorizationDataAsync(IUserIdentityEntity userRecord)
        {
            var profile = await profileRepository.Get(userRecord.ProfileId);

            var profileAdapter = UserProfileAdapterFactory.CreateProfileAdapter(profile);

            var userData = new AuthorizationData()
            {
                Id = userRecord.Id,
                DisplayName = profileAdapter.DisplayName,
                DisplayImageUrl = profileAdapter.DisplayProfileImageUrl,
                Role = profileAdapter.UserRole
            };

            return userData;
        }
    }
}
