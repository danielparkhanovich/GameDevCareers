using JobBoardPlatform.BLL.DTOs;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public interface IRegistrationInteractor<T> where T : UserLoginData
    {
        Task<RedirectData> ProcessRegistrationAndRedirect(T model);
        Task FinishRegistration(string tokenId, HttpContext httpContext);
    }
}
