using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class LoadActualOffersPage : ILoader<List<JobOffer>>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly int page;
        private readonly int pageSize;

        public int SelectedOffersCount { get; private set; }


        public LoadActualOffersPage(IRepository<JobOffer> repository, 
            int page, int pageSize)
        {
            this.repository = repository;
            this.page = page;
            this.pageSize = pageSize;
        }

        public async Task<List<JobOffer>> Load()
        {
            var offersSet = await repository.GetAllSet();

            var filtered = offersSet.Where(offer => 
                (!offer.IsDeleted && 
                 offer.IsPaid && 
                 !offer.IsSuspended && 
                 !offer.IsShelved && 
                 offer.IsPublished));

            var offers = await filtered
                .OrderByDescending(offer => offer.PublishedAt)
                .Include(offer => offer.CompanyProfile)
                .Include(offer => offer.WorkLocation)
                .Include(offer => offer.MainTechnologyType)
                .Include(offer => offer.TechKeywords)
                .Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange != null ? details.SalaryRange.SalaryCurrency : null)
                .Include(offer => offer.ContactDetails)
                    .ThenInclude(details => details.ContactType)
                .ToListAsync();

            this.SelectedOffersCount = offers.Count();

            var pageOffers = offers
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            return pageOffers;
        }
    }
}
