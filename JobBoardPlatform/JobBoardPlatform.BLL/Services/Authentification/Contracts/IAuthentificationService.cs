using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IAuthentificationService<T> 
        where T : class, IUserIdentityEntity
    {
        Task<T> TryRegisterAsync(T credentials);
        Task<T> TryLoginAsync(T credentials);
    }
}
