using JobBoardPlatform.BLL.Query.Identity;
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
        private readonly IOfferQueryExecutor queryExecutor;
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
            IOfferQueryExecutor queryExecutor,
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
            this.queryExecutor = queryExecutor;
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
            var offer = await queryExecutor.GetOfferById(offerIdToDelete);

            await UnassignResumesFromOffer(offer);
            await offersRepository.Delete(offerIdToDelete);
            await DeleteOfferContent(offer);
        }

        private async Task DeleteOfferContent(JobOffer offer)
        {
            await DeleteEmploymentDetails(offer);
            await DeleteTechKeywords(offer);
            var test = await contactDetailsRepository.Get(offer.ContactDetailsId);
            await contactDetailsRepository.Delete(offer.ContactDetailsId);
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
