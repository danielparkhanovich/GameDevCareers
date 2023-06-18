using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Session
{
    public interface IUserSessionService<T, V>
    {
        Task UpdateSessionStateAsync(HttpContext httpContext);
    }
}
