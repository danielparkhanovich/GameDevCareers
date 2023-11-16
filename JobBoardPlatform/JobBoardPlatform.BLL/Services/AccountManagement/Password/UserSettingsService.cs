using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Password
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IModifyIdentityService<EmployeeIdentity> employeeModifyService;
        private readonly IModifyIdentityService<CompanyIdentity> companyModifyService;
        private readonly UserManager<EmployeeIdentity> employeeManager;
        private readonly UserManager<CompanyIdentity> companyManager;


        public UserSettingsService(
            IModifyIdentityService<EmployeeIdentity> employeeModifyService,
            IModifyIdentityService<CompanyIdentity> companyModifyService,
            UserManager<EmployeeIdentity> employeeManager,
            UserManager<CompanyIdentity> companyManager)
        {
            this.employeeModifyService = employeeModifyService;
            this.companyModifyService = companyModifyService;
            this.employeeManager = employeeManager;
            this.companyManager = companyManager;
        }

        public async Task<string> GetLoginAsync(string role, int identityId)
        {
            if (role == UserRoles.Employee || role == UserRoles.Admin)
            {
                return (await employeeManager.GetAsync(identityId)).Email;
            }
            else if (role == UserRoles.Company)
            {
                return (await companyManager.GetAsync(identityId)).Email;
            }
            else
            {
                throw new Exception("Unsupported user role!");
            }
        }

        public Task DeleteAccountAsync(string role, int identityId)
        {
            if (role == UserRoles.Employee || role == UserRoles.Admin)
            {
                return employeeManager.DeleteAsync(identityId);
            }
            else if (role == UserRoles.Company)
            {
                return companyManager.DeleteAsync(identityId);
            }
            else
            {
                throw new Exception("Unsupported user role!");
            }
        }

        public async Task TryUpdateLoginDataAsync(string role, int identityId, LoginSettingsData loginSettings)
        {
            if (role == UserRoles.Employee || role == UserRoles.Admin)
            {
                await TryUpdateLoginDataAsync(identityId, loginSettings, employeeModifyService, employeeManager);
            }
            else if (role == UserRoles.Company)
            {
                await TryUpdateLoginDataAsync(identityId, loginSettings, companyModifyService, companyManager);
            }
            else
            {
                throw new Exception("Unsupported user role!");
            }
        }

        private async Task TryUpdateLoginDataAsync<TIdentity>(
            int identityId,
            LoginSettingsData loginSettings, 
            IModifyIdentityService<TIdentity> modifyService,
            UserManager<TIdentity> userManager) 
            where TIdentity : class, IUserIdentityEntity
        {
            var user = await userManager.GetAsync(identityId);
            if (IsChangeLogin(user.Email, loginSettings.Login))
            {
                await modifyService.TryChangeLoginAsync(identityId, loginSettings.Login!);
            }
            else if (IsChangePassword(loginSettings.OldPassword, loginSettings.NewPassword))
            {
                await modifyService.TryChangePasswordAsync(identityId, loginSettings.OldPassword!, loginSettings.NewPassword!);
            }
        }

        private bool IsChangeLogin(string oldLogin, string? newLogin)
        {
            return !string.IsNullOrEmpty(newLogin) && oldLogin != newLogin;
        }

        private bool IsChangePassword(string? oldPassword, string? newPassword)
        {
            return !string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword);
        }
    }
}
