using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface ILoginService<T, V> where T : class, IUserIdentityEntity
    {
        Task<T> TryLoginAsync(string email, string password, HttpContext httpContext);
        Task<T> ForceLoginAsync(string email, HttpContext httpContext);
    }

    public interface ILoginService<T> where T : class, IUserIdentityEntity
    {
        Task<T> TryLoginAsync(string email, string password, HttpContext httpContext);
    }
}
