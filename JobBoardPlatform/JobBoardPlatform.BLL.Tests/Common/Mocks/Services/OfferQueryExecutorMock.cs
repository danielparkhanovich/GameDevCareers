using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.Services
{
    internal class OfferQueryExecutorMock : IOfferQueryExecutor
    {
        private readonly IRepository<JobOffer> repository;


        public OfferQueryExecutorMock(IRepository<JobOffer> repository)
        {
            this.repository = repository;
        }

        public Task<JobOffer> GetOfferById(int id)
        {
            return repository.Get(id);
        }
    }
}
