using JobBoardPlatform.DAL.Models.Company;
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
        private readonly IRepository<JobOfferContactDetails> contactDetailsRepository;
        private readonly IRepository<JobOfferEmploymentDetails> employmentDetailsRepository;
        private readonly IRepository<JobOfferSalariesRange> salariesRangeRepository;
        private readonly IRepository<JobOfferTechKeyword> techKeywordsRepository;
        private readonly IRepository<JobOfferApplication> applicationsRepository;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage applicationsResumeStorage;
        private readonly int offerIdToDelete;


        public DeleteOfferCommand(
            IRepository<JobOffer> offersRepository,
            IRepository<JobOfferContactDetails> contactDetailsRepository,
            IRepository<JobOfferEmploymentDetails> employmentDetailsRepository,
            IRepository<JobOfferSalariesRange> salariesRangeRepository,
            IRepository<JobOfferTechKeyword> techKeywordsRepository,
            IRepository<JobOfferApplication> applicationsRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage applicationsResumeStorage,
            int offerIdToDelete)
        {
            this.offersRepository = offersRepository;
            this.contactDetailsRepository = contactDetailsRepository;
            this.employmentDetailsRepository = employmentDetailsRepository;
            this.salariesRangeRepository = salariesRangeRepository;
            this.applicationsRepository = applicationsRepository;
            this.techKeywordsRepository = techKeywordsRepository;
            this.profileResumeStorage = profileResumeStorage;
            this.applicationsResumeStorage = applicationsResumeStorage;
            this.offerIdToDelete = offerIdToDelete;
        }

        public async Task Execute()
        {
            var offer = await offersRepository.Get(offerIdToDelete);

            await UnassignResumesFromOffer(offer);
            await offersRepository.Delete(offerIdToDelete);
            await DeleteOfferContent(offer);
        }

        private async Task DeleteOfferContent(JobOffer offer)
        {
            await contactDetailsRepository.Delete(offer.ContactDetailsId);
            await DeleteEmploymentDetails(offer);
            await DeleteTechKeywords(offer);
        }

        private async Task DeleteEmploymentDetails(JobOffer offer)
        {
            var employmentDetailsIds = offer.EmploymentDetails.Select(x => x.Id);
            foreach (var id in employmentDetailsIds)
            {
                var employmentDetails = await employmentDetailsRepository.Get(id);
                if (employmentDetails.SalaryRangeId.HasValue)
                {
                    await salariesRangeRepository.Delete(employmentDetails.SalaryRangeId.Value);
                }
                await employmentDetailsRepository.Delete(id);
            }
        }

        private async Task DeleteTechKeywords(JobOffer offer)
        {
            if (offer.TechKeywords == null)
            {
                return;
            }

            var techKeywordsIds = offer.TechKeywords.Select(x => x.Id);
            foreach (var id in techKeywordsIds)
            {
                await techKeywordsRepository.Delete(id);
            }
        }

        private async Task UnassignResumesFromOffer(JobOffer offer)
        {
            if (offer.OfferApplications == null)
            {
                return;
            }

            foreach (var application in offer.OfferApplications) 
            {
                var deleteApplicationCommand = new DeleteApplicationCommand(
                        application.Id,
                        applicationsRepository,
                        profileResumeStorage,
                        applicationsResumeStorage);
                await deleteApplicationCommand.Execute();
            }
        }
    }
}
