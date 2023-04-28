using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class LoadOfferContent : ILoader<JobOffer>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly int offerId;


        public LoadOfferContent(IRepository<JobOffer> repository, int offerId)
        {
            this.repository = repository;
            this.offerId = offerId;
        }

        public async Task<JobOffer> Load()
        {
            var offersSet = await repository.GetAllSet();
            var offer = await offersSet.Where(x => x.Id == offerId)
                .Include(x => x.CompanyProfile)
                .Include(x => x.WorkLocation)
                .Include(x => x.MainTechnologyType)
                .Include(x => x.TechKeywords)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.SalaryRange != null ? y.SalaryRange.SalaryCurrency : null)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.EmploymentType)
                .Include(x => x.ContactDetails)
                    .ThenInclude(y => y.ContactType)
                .SingleAsync();

            return offer;
        }
    }
}
