using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IRegistrationService<T> where T : class, IUserIdentityEntity, new()
    {
        Task<T> TryRegisterAsync(string email, string password);
        Task<T> TryRegisterAsync(T identity, string password);
    }
}
