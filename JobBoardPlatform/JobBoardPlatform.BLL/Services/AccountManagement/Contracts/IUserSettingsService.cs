using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IUserSettingsService
    {
        Task<string> GetLoginAsync(string role, int identityId);
        Task TryUpdateLoginDataAsync(string role, int identityId, ILoginSettingsData loginSettings);
        Task DeleteAccountAsync(string role, int identityId);
    }
}
