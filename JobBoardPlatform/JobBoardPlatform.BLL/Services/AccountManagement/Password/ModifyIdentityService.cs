using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password
{
    public class ModifyIdentityService<T> : IModifyIdentityService<T>
        where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> identityRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly UserManager<T> userManager;


        public ModifyIdentityService(
            IRepository<T> repository, IPasswordHasher passwordHasher, UserManager<T> userManager)
        {
            identityRepository = repository;
            this.passwordHasher = passwordHasher;
            this.userManager = userManager;
        }

        public async Task<IUserIdentityEntity> TryChangeLoginAsync(int userId, string newLogin)
        {
            await ValidateNewLogin(newLogin);

            var user = await identityRepository.Get(userId);
            return await UpdateUserLogin(user, newLogin);
        }

        public async Task<IUserIdentityEntity> TryChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await identityRepository.Get(userId);
            ValidatePassword(oldPassword, newPassword);
            await UpdateUserPassword(user, newPassword);
            return user;
        }

        public async Task<IUserIdentityEntity> ForceChangePasswordAsync(string email, string password)
        {
            var user = await userManager.GetWithEmailAsync(email);
            await UpdateUserPassword(user, password);
            return user;
        }

        private async Task<T> UpdateUserLogin(T user, string newLogin)
        {
            user.Email = newLogin;
            return await identityRepository.Update(user);
        }

        private Task UpdateUserPassword(T user, string newPassword)
        {
            user.HashPassword = passwordHasher.GetHash(newPassword);
            return identityRepository.Update(user);
        }

        private async Task ValidateNewLogin(string newLogin)
        {
            if (await userManager.GetWithEmailAsync(newLogin) != null)
            {
                throw new AuthenticationException(AuthenticationException.WrongEmail);
            }
        }

        private void ValidatePassword(string oldPassword, string newPassword)
        {
            string oldPasswordHash = passwordHasher.GetHash(oldPassword);
            string newPasswordHash = passwordHasher.GetHash(newPassword);
            if (oldPasswordHash != newPasswordHash)
            {
                throw new AuthenticationException(AuthenticationException.WrongPassword);
            }
        }
    }
}
