using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Query.Identity
{
    public class OfferQueryExecutor
    {
        private readonly IRepository<JobOffer> repository;


        public OfferQueryExecutor(IRepository<JobOffer> repository)
        {
            this.repository = repository;
        }

        public Task<JobOffer> GetOfferById(int id)
        {
            var loader = new OfferQueryLoader();
            var query = new GetEntityByIdQuery<JobOffer>(repository, loader, id);
            return query.Get();
        }
    }
}
