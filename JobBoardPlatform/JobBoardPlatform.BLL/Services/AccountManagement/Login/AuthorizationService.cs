using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Login
{
    public class AuthorizationService<TEntity, TProfile> : IAuthorizationService<TEntity, TProfile>
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly IRepository<TEntity> userRepository;
        private readonly IRepository<TProfile> profilesRepository;
        private readonly AuthorizationServiceCore authorization;


        public AuthorizationService(
            IRepository<TEntity> userRepository,
            IRepository<TProfile> profilesRepository,
            AuthorizationServiceCore serviceCore)
        {
            this.userRepository = userRepository;
            this.profilesRepository = profilesRepository;
            this.authorization = serviceCore;
        }

        public async Task SignInHttpContextAsync(HttpContext httpContext, int userId)
        {
            if (UserSessionUtils.IsLoggedIn(httpContext.User))
            {
                await SignOutHttpContextAsync(httpContext);
            }

            var data = await GetAuthorizationDataAsync(userId);
            await authorization.SignInHttpContextAsync(httpContext, data);
        }

        public async Task SignOutHttpContextAsync(HttpContext httpContext)
        {
            await authorization.SignOutHttpContextAsync(httpContext);
        }

        private async Task<AuthorizationData> GetAuthorizationDataAsync(int userId)
        {
            var user = await userRepository.Get(userId);
            var profileRecord = await profilesRepository.Get(user.ProfileId);
            return new AuthorizationData(user.Id, user.Email, profileRecord);
        }
    }
}
