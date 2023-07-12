using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public interface IRegistrationInteractor<T> where T : class, IUserLoginData
    {
        Task<RedirectData> ProcessRegistrationAndRedirect(T model);
        Task FinishRegistration(string tokenId, HttpContext httpContext);
    }
}
