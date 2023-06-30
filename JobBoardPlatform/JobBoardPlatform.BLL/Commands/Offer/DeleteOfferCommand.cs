using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteOfferCommand : ICommand
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage applicationsResumeStorage;
        private readonly int offerIdToDelete;


        public DeleteOfferCommand(
            IRepository<JobOffer> offersRepository, 
            IRepository<OfferApplication> applicationsRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage applicationsResumeStorage,
            int offerIdToDelete)
        {
            this.offersRepository = offersRepository;
            this.applicationsRepository = applicationsRepository;
            this.profileResumeStorage = profileResumeStorage;
            this.applicationsResumeStorage = applicationsResumeStorage;
            this.offerIdToDelete = offerIdToDelete;
        }

        public async Task Execute()
        {
            var offer = await offersRepository.Get(offerIdToDelete);
            await DeleteDataFromFileStorages(offer);
            await offersRepository.Delete(offerIdToDelete);
        }

        private async Task DeleteDataFromFileStorages(JobOffer offer)
        {
            if (offer.OfferApplications == null)
            {
                return;
            }

            var applications = offer.OfferApplications;
            foreach (var application in applications) 
            {
                await profileResumeStorage.UnassignFromOfferOnOfferClosedAsync(offer.Id, application.ResumeUrl);
                await applicationsResumeStorage.UnassignFromOfferOnOfferClosedAsync(offer.Id, application.ResumeUrl);
            }
        }
    }
}
