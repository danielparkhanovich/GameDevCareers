using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Company;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public interface IApplicationsManager
    {
        Task<ICollection<JobOfferApplication>> GetApplicationsAsync(int offerId); 
        Task<JobOfferApplication> GetApplicationAsync(int applicationId);
        Task TryPostApplicationFormAsync(
            int offerId, int? userProfileId, HttpRequest request, HttpResponse response, IApplicationForm form, IEmailContent<JobOfferApplication> emailContent);
        Task<int> UpdateApplicationPriorityAsync(int applicationId, int newPriorityIndex);
    }
}
