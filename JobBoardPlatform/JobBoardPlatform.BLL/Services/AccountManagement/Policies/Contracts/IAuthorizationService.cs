using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts
{
    public interface IAuthorizationService<TEntity, TProfile>
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        Task SignInHttpContextAsync(HttpContext httpContext, int userId);
        Task SignOutHttpContextAsync(HttpContext httpContext);
    }
}
