using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.DAL.Contexts
{
    public class OfferContext
    {
        public IRepository<JobOffer> OffersRepository { get; }
        public IRepository<JobOfferContactDetails> ContactDetailsRepository { get; }
        public IRepository<JobOfferEmploymentDetails> EmploymentDetailsRepository { get; }
        public IRepository<JobOfferSalariesRange> SalariesRangeRepository { get; }
        public IRepository<JobOfferTechKeyword> TechKeywordsRepository { get; }
        public IRepository<JobOfferApplication> ApplicationsRepository { get; }
        public IProfileResumeBlobStorage ProfileResumeStorage { get; }
        public IApplicationsResumeBlobStorage ApplicationsResumeStorage { get; }


        public OfferContext(
            IRepository<JobOffer> offersRepository,
            IRepository<JobOfferContactDetails> contactDetailsRepository,
            IRepository<JobOfferEmploymentDetails> employmentDetailsRepository,
            IRepository<JobOfferSalariesRange> salariesRangeRepository,
            IRepository<JobOfferTechKeyword> techKeywordsRepository,
            IRepository<JobOfferApplication> applicationsRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage applicationsResumeStorage)
        {
            this.OffersRepository = offersRepository;
            this.ContactDetailsRepository = contactDetailsRepository;
            this.EmploymentDetailsRepository = employmentDetailsRepository;
            this.SalariesRangeRepository = salariesRangeRepository;
            this.ApplicationsRepository = applicationsRepository;
            this.TechKeywordsRepository = techKeywordsRepository;
            this.ProfileResumeStorage = profileResumeStorage;
            this.ApplicationsResumeStorage = applicationsResumeStorage;
        }

        public async Task DeleteEmploymentDetails(JobOffer offer)
        {
            var employmentDetailsIds = offer.EmploymentDetails.Select(x => x.Id);
            foreach (var id in employmentDetailsIds)
            {
                var employmentDetails = await EmploymentDetailsRepository.Get(id);
                if (employmentDetails.SalaryRangeId.HasValue)
                {
                    await SalariesRangeRepository.Delete(employmentDetails.SalaryRangeId.Value);
                }
                await EmploymentDetailsRepository.Delete(id);
            }
        }

        public async Task DeleteTechKeywords(JobOffer offer)
        {
            if (offer.TechKeywords == null)
            {
                return;
            }

            var techKeywordsIds = offer.TechKeywords.Select(x => x.Id);
            foreach (var id in techKeywordsIds)
            {
                await TechKeywordsRepository.Delete(id);
            }
        }
    }
}
