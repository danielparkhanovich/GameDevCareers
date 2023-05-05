using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class LoadActualOffersPage : ILoader<List<JobOffer>>
    {
        private readonly IQueryable<JobOffer> offersQuery;


        public LoadActualOffersPage(IQueryable<JobOffer> offersQuery)
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
