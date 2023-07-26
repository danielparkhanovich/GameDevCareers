using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class DeleteApplicationCommand : ICommand
    {
        private readonly int applicationId;
        private readonly IRepository<JobOfferApplication> applicationsRepository;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage applicationsResumeStorage;


        public DeleteApplicationCommand(
            int applicationId,
            IRepository<JobOfferApplication> applicationsRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage applicationsResumeStorage)
        {
            this.applicationId = applicationId;
            this.applicationsRepository = applicationsRepository;
            this.profileResumeStorage = profileResumeStorage;
            this.applicationsResumeStorage = applicationsResumeStorage;
        }

        public async Task Execute()
        {
            var application = await applicationsRepository.Get(applicationId);
            await UnassignResumesFromOffer(application);
            await applicationsRepository.Delete(applicationId);
        }

        private async Task UnassignResumesFromOffer(JobOfferApplication application)
        {
            int offerId = application.JobOfferId;
            await profileResumeStorage.UnassignFromOfferOnOfferClosedAsync(offerId, application.ResumeUrl);
            await applicationsResumeStorage.UnassignFromOfferOnOfferClosedAsync(offerId, application.ResumeUrl);
        }
    }
}
