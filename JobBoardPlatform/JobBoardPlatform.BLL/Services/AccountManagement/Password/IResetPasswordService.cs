
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password
{
    public interface IResetPasswordService<TEntity, TProfile>
        where TEntity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
    {
        Task TrySendResetPasswordTokenAsync(string email);
        Task TryChangePasswordByTokenAsync(string tokenId, string newPassword, HttpContext httpContext);
    }
}
