using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class LoadCompanyOffers : ILoader<List<JobOffer>>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly int profileId;


        public LoadCompanyOffers(IRepository<JobOffer> repository, 
            int companyProfileId)
        {
            this.repository = repository;
            this.profileId = companyProfileId;
        }

        public async Task<List<JobOffer>> Load()
        {
            var offersSet = await repository.GetAllSet();
            var offers = await offersSet.Where(offer => offer.CompanyProfileId == profileId)
                .OrderByDescending(offer => offer.CreatedAt)
                .Include(offer => offer.CompanyProfile)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .Include(offer => offer.ContactDetails)
                    .ThenInclude(details => details.ContactType)
                .ToListAsync();

            return offers;
        }
    }
}
