using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    public class LogIntoAccountCommand<TIdentity, TProfile> : ICommand
        where TIdentity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        private readonly AuthorizationService authorizationService;
        private readonly IRepository<TIdentity> identityRepository;
        private readonly IRepository<TProfile> profileRepository;
        private readonly int idLogInto;


        public LogIntoAccountCommand(
            HttpContext httpContext, 
            IRepository<TIdentity> identityRepository,
            IRepository<TProfile> profileRepository,
            int idLogInto)
        {
            this.authorizationService = new AuthorizationService(httpContext);
            this.identityRepository = identityRepository;
            this.profileRepository = profileRepository;
            this.idLogInto = idLogInto;
        }

        public async Task Execute()
        {
            await SignOut();
            await SignIntoAccount();
        }

        private Task SignOut()
        {
            return authorizationService.SignOutHttpContextAsync();
        }

        private async Task SignIntoAccount()
        {
            var authorizationData = await GetAuthorizationDataAsync(idLogInto);
            await authorizationService.SignInHttpContextAsync(authorizationData);
        }

        private async Task<AuthorizationData> GetAuthorizationDataAsync(int idLogInto)
        {
            var identity = await identityRepository.Get(idLogInto);
            var profile = await profileRepository.Get(identity.ProfileId);
            return new AuthorizationData(idLogInto, identity.Email, profile);
        }
    }
}
