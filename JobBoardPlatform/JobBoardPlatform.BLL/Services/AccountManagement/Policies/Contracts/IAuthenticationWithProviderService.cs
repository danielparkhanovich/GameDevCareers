using JobBoardPlatform.BLL.Services.Authentification.Authorization.IdentityProviders;
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts
{
    public interface IAuthenticationWithProviderService<T> where T : IUserIdentityEntity
    {
        Task<T> TryLoginOrRegisterAsync(HttpContext httpContext, IIdentityProviderClaimKeys claimKeys);
    }
}
