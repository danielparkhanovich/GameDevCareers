using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.Services
{
    internal class OffersCacheManagerMock : IOffersCacheManager
    {
        public Task<EntitiesFilteringSearchResponse<JobOffer>> GetOffersFromCache()
        {
            return Task.FromResult(new EntitiesFilteringSearchResponse<JobOffer>());
        }

        public Task UpdateCache(EntitiesFilteringSearchResponse<JobOffer> searchResponse)
        {
            return Task.CompletedTask;
        }
    }
}
