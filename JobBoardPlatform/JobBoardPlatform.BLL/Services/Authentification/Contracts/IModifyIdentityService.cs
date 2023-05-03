using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IModifyIdentityService<T> 
        where T : class, IUserIdentityEntity
    {
        Task<AuthentificationResult> TryChangeLoginIdentifier(T credentials, string newLogin);
        Task<AuthentificationResult> ChangePassword(T credentials, string oldPassword, string newPassword);
    }
}
