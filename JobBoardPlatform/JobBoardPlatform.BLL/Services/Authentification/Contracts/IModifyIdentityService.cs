using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IModifyIdentityService<T> 
        where T : class, IUserIdentityEntity
    {
        Task<IUserIdentityEntity> TryChangeLoginIdentifierAsync(T credentials, string newLogin);
        Task<IUserIdentityEntity> TryChangePasswordAsync(T credentials, string oldPassword, string newPassword);
    }
}
