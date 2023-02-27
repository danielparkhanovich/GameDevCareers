using JobBoardPlatform.BLL.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IProcessAutorization<T> 
        where T : class, ICredentialEntity
    {
        Task<AuthorizationResult> TryRegister(T credentials);
        Task<AuthorizationResult> TryLogin(T credentials);
    }
}
