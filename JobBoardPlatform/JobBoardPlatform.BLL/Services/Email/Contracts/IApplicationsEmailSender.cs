using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Services.IdentityVerification.Contracts
{
    public interface IApplicationsEmailSender
    {
        Task SendEmailAsync(string targetEmail, JobOfferApplication application);
    }
}
