using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IModifyIdentityService<T> 
        where T : class, IUserIdentityEntity
    {
        Task<IUserIdentityEntity> TryChangeLoginIdentifierAsync(int userId, string newLogin, string currentPassword);
        Task<IUserIdentityEntity> TryChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
