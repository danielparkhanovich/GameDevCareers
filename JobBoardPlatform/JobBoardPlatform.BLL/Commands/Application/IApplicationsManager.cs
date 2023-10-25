using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Company;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public interface IApplicationsManager
    {
        Task<ICollection<JobOfferApplication>> GetApplicationsAsync(int offerId); 
        Task<JobOfferApplication> GetApplicationAsync(int applicationId);
        Task PostApplicationFormAsync(
            int offerId, int? userProfileId, ApplicationForm form, IEmailContent<JobOfferApplication> emailContent);
        Task RedirectApplicationFormAsync(int offerId);
        Task<int> UpdateApplicationPriorityAsync(int applicationId, int newPriorityIndex);
    }
}
