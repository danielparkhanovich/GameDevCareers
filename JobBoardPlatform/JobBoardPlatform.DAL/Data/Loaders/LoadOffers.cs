using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    /// <summary>
    /// Loads data required to display offer card
    /// </summary>
    public class LoadOffers : ILoader<List<JobOffer>>
    {
        private readonly IQueryable<JobOffer> offersQuery;


        public LoadOffers(IQueryable<JobOffer> offersQuery)
        {
            this.offersQuery = offersQuery;
        }

        public async Task<List<JobOffer>> Load()
        {
            var offers = await offersQuery
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
