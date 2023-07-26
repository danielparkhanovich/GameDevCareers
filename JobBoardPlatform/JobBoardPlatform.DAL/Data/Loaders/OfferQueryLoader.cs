using JobBoardPlatform.DAL.Models.Company;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class OfferQueryLoader : IEntityLoader<JobOffer>
    {
        public IQueryable<JobOffer> Load(IQueryable<JobOffer> queryable)
        {
            var offer = queryable
                .Include(x => x.CompanyProfile)
                .Include(x => x.WorkLocation)
                .Include(x => x.MainTechnologyType)
                .Include(x => x.TechKeywords)
                .Include(x => x.EmploymentDetails)
                    .ThenInclude(y => y.SalaryRange != null ? y.SalaryRange.SalaryCurrency : null)
                .Include(x => x.EmploymentDetails)
                    .ThenInclude(y => y.EmploymentType)
                .Include(x => x.ContactDetails)
                    .ThenInclude(y => y.ContactType);
            return offer;
        }
    }
}
