using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IModifyIdentityService<T> 
        where T : class, IUserIdentityEntity
    {
        Task<IUserIdentityEntity> TryChangeLoginAsync(int userId, string newLogin);
        Task<IUserIdentityEntity> TryChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<IUserIdentityEntity> ForceChangePasswordAsync(string email, string password);
    }
}
