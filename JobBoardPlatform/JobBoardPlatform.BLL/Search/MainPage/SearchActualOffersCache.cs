using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Cache;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    /// <summary>
    /// Searcher with destributed cache on the top
    /// </summary>
    public class SearchActualOffersCache : ISearcher<JobOffer, MainPageOfferSearchParameters>
    {
        private readonly ICacheRepository<List<JobOffer>> offersCache;
        private readonly ICacheRepository<int> offersCountCache;
        private readonly MainPageOffersSearcher searcher;

        public int AfterFiltersCount { get; set; }
        public MainPageOfferSearchParameters SearchParams { get; set; }


        public SearchActualOffersCache(MainPageOfferSearchParameters searchParams,
            ICacheRepository<List<JobOffer>> offersCache,
            ICacheRepository<int> offersCountCache)
        {
            this.searcher = new MainPageOffersSearcher(searchParams);
            this.SearchParams = searchParams;
            this.offersCache = offersCache;
            this.offersCountCache = offersCountCache;
        }

        public async Task<List<JobOffer>> Search(IRepository<JobOffer> repository)
        {
            if (SearchParams.IsQueryParams)
            {
                return await GetOffersFromRepository(repository);
            }
            else
            {
                return await TryGetOffersFromCache(repository);
            }
        }

        private async Task<List<JobOffer>> TryGetOffersFromCache(IRepository<JobOffer> repository)
        {
            var offers = new List<JobOffer>();

            try
            {
                offers = await offersCache.GetAsync();
                AfterFiltersCount = await offersCountCache.GetAsync();
            }
            catch (Exception ex)
            {
                offers = await GetOffersFromRepository(repository);
                await UpdateCache(offers);
            }
            return offers;
        }

        private async Task<List<JobOffer>> GetOffersFromRepository(IRepository<JobOffer> repository)
        {
            List<JobOffer> offers = await searcher.Search(repository);
            AfterFiltersCount = searcher.AfterFiltersCount;
            return offers;
        }

        private async Task UpdateCache(List<JobOffer> offers)
        {
            await offersCache.UpdateAsync(offers);
            await offersCountCache.UpdateAsync(AfterFiltersCount);
        }
    }
}
