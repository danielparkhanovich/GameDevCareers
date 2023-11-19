using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public interface IApplicationsManager
    {
        Task<ICollection<JobOfferApplication>> GetApplicationsAsync(int offerId); 
        Task<JobOfferApplication> GetApplicationAsync(int applicationId);
        Task PostApplicationFormAsync(
            int offerId, int? userProfileId, ApplicationForm form, bool isSendEmail = true);
        Task RedirectApplicationFormAsync(int offerId);
        Task<int> UpdateApplicationPriorityAsync(int applicationId, int newPriorityIndex);
    }
}
