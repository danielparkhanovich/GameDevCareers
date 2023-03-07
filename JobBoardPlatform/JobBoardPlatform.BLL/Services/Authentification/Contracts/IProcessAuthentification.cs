using JobBoardPlatform.BLL.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IProcessAuthentification<T> 
        where T : class, ICredentialEntity
    {
        Task<AuthorizationResult> TryRegisterAsync(T credentials);
        Task<AuthorizationResult> TryLoginAsync(T credentials);
    }
}
