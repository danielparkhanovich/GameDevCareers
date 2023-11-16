using JobBoardPlatform.BLL.DTOs;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IUserSettingsService
    {
        Task<string> GetLoginAsync(string role, int identityId);
        Task TryUpdateLoginDataAsync(string role, int identityId, LoginSettingsData loginSettings);
        Task DeleteAccountAsync(string role, int identityId);
    }
}
