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
        private readonly OffersCacheManager cacheManager;

        public MainPageOfferSearchParams SearchParams { get; set; }


        public MainPageOffersSearcherCacheDecorator(MainPageOffersSearcher searcher,
            OffersCacheManager cacheManager)
        {
            this.SearchParams = searcher.SearchParams;
            this.searcher = searcher;
            this.cacheManager = cacheManager;
        }

        public async Task<EntitiesFilteringSearchResponse<JobOffer>> Search()
        {
            if (SearchParams.IsQueryParams)
            {
                return await GetOffersFromRepository();
            }
            else
            {
                return await TryGetOffersFromCache();
            }
        }

        private async Task<EntitiesFilteringSearchResponse<JobOffer>> TryGetOffersFromCache()
        {
            try
            {
                return await cacheManager.GetOffersFromCache();
            }
            catch (Exception ex)
            {
                var searchResponse = await searcher.Search();
                await cacheManager.UpdateCache(searchResponse);
                return searchResponse;
            }
        }

        private Task<EntitiesFilteringSearchResponse<JobOffer>> GetOffersFromRepository()
        {
            return searcher.Search();
        }
    }
}
