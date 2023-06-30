using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public interface IOffersCacheManager
    {
        Task<EntitiesFilteringSearchResponse<JobOffer>> GetOffersFromCache();
        Task UpdateCache(EntitiesFilteringSearchResponse<JobOffer> searchResponse);
    }
}
