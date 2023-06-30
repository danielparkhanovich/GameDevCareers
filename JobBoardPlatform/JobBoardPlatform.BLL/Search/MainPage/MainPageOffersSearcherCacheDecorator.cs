using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    /// <summary>
    /// Searcher with destributed cache on the top
    /// </summary>
    public class MainPageOffersSearcherCacheDecorator : IFilteringSearcher<JobOffer, MainPageOfferSearchParams>
    {
        private readonly MainPageOffersSearcher searcher;
        private readonly IOffersCacheManager cacheManager;


        public MainPageOffersSearcherCacheDecorator(
            MainPageOffersSearcher searcher, IOffersCacheManager cacheManager)
        {
            this.searcher = searcher;
            this.cacheManager = cacheManager;
        }

        public async Task<EntitiesFilteringSearchResponse<JobOffer>> Search(MainPageOfferSearchParams searchParams)
        {
            if (searchParams.IsQueryParams)
            {
                return await GetOffersFromRepository(searchParams);
            }
            else
            {
                return await TryGetOffersFromCache(searchParams);
            }
        }

        private async Task<EntitiesFilteringSearchResponse<JobOffer>> TryGetOffersFromCache(
            MainPageOfferSearchParams searchParams)
        {
            try
            {
                return await cacheManager.GetOffersFromCache();
            }
            catch (Exception ex)
            {
                var searchResponse = await searcher.Search(searchParams);
                await cacheManager.UpdateCache(searchResponse);
                return searchResponse;
            }
        }

        private Task<EntitiesFilteringSearchResponse<JobOffer>> GetOffersFromRepository(
            MainPageOfferSearchParams searchParams)
        {
            return searcher.Search(searchParams);
        }
    }
}
