using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authorization
{
    public class SessionService<TCredential, TProfile> : IAuthentificationService<TCredential>
        where TCredential : class, ICredentialEntity
        where TProfile : class, IEntity, IDisplayData
    {
        private readonly IRepository<TProfile> profileRepository;
        private readonly IAuthentificationService<TCredential> authentificationService;
        private readonly IAuthorizationService authorizationService;
        private readonly string role;


        public SessionService(HttpContext httpContext, IRepository<TCredential> userRepository, 
            IRepository<TProfile> profileRepository, string role)
        {
            this.profileRepository = profileRepository;
            this.authentificationService = new AuthentificationService<TCredential>(userRepository);
            this.authorizationService = new AuthorizationService(httpContext);
            this.role = role;
        }

        public async Task<AuthentificationResult> TryLoginAsync(TCredential credentials)
        {
            var authentificationFunc = authentificationService.TryLoginAsync;

            return await TryAuthorize(authentificationFunc, credentials);
        }

        public async Task<AuthentificationResult> TryRegisterAsync(TCredential credentials)
        {
            var authentificationFunc = authentificationService.TryRegisterAsync;

            return await TryAuthorize(authentificationFunc, credentials);
        }

        private async Task<AuthentificationResult> TryAuthorize(Func<TCredential, Task<AuthentificationResult>> authentificationFunc,
            TCredential credentials)
        {
            var authentification = await authentificationFunc(credentials);
            if (authentification.IsError)
            {
                return authentification;
            }

            var userData = await GetAuthorizationDataAsync(authentification.FoundRecord!);

            await authorizationService.SignInHttpContextAsync(userData);

            return AuthentificationResult.Success;
        }

        private async Task<AuthorizationData> GetAuthorizationDataAsync(ICredentialEntity userRecord)
        {
            var profile = await profileRepository.Get(userRecord.ProfileId);

            var userData = new AuthorizationData()
            {
                Id = userRecord.ProfileId,
                DisplayName = profile.DisplayName,
                DisplayImageUrl = profile.DisplayImageUrl,
                NameIdentifier = userRecord.Email,
                Role = role
            };

            return userData;
        }
    }
}
