using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IAuthentificationService<T> 
        where T : class, ICredentialEntity
    {
        Task<AuthentificationResult> TryRegisterAsync(T credentials);
        Task<AuthentificationResult> TryLoginAsync(T credentials);
    }
}
