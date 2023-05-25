using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Search.CompanyPanel
{
    public class CompanyOffersSearcher : ISearcher<JobOffer, CompanyPanelOfferSearchParameters>
    {
        public CompanyPanelOfferSearchParameters SearchParams { get; set; }
        public int AfterFiltersCount { get; set; }


        public CompanyOffersSearcher(CompanyPanelOfferSearchParameters searchParams)
        {
            SearchParams = searchParams;
            AfterFiltersCount = 0;
        }

        public async Task<List<JobOffer>> Search(IRepository<JobOffer> repository)
        {
            var offersSet = await repository.GetAllSet();
            var offers = offersSet;

            var filtered = GetFiltered(offers);
            AfterFiltersCount = filtered.Count();

            var sorted = GetSorted(filtered);

            int page = SearchParams.Page;
            int pageSize = SearchParams.PageSize;

            var pageOffers = sorted.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var loader = new LoadOffers(pageOffers);
            var loaded = await loader.Load();

            return loaded;
        }

        private IQueryable<JobOffer> GetFiltered(IQueryable<JobOffer> available)
        {
            if (SearchParams.CompanyProfileId != null)
            {
                available = available.Where(offer => offer.CompanyProfileId == SearchParams.CompanyProfileId);
            }

            available = available.Where(offer =>
                 offer.IsPublished && !offer.IsShelved && !offer.IsDeleted && !offer.IsSuspended && SearchParams.IsShowPublished ||
                 (offer.IsShelved || offer.IsDeleted || offer.IsSuspended || !offer.IsPaid) && SearchParams.IsShowShelved);

            return available;
        }

        private IQueryable<JobOffer> GetSorted(IQueryable<JobOffer> available)
        {
            if (SearchParams.SortCategory == SortCategoryType.PublishDate.ToString())
            {
                available = available.OrderByDescending(x => x.CreatedAt);
            }
            else if (SearchParams.SortCategory == SortCategoryType.Alphabetically.ToString())
            {
                available = available.OrderByDescending(x => x.JobTitle);
            }
            else if (SearchParams.SortCategory == SortCategoryType.Relevenacy.ToString())
            {
                available = available.OrderByDescending(x => x.NumberOfViews + x.NumberOfApplications * 2);
            }

            if (SearchParams.Sort == SortType.Descending)
            {
                available = available.Reverse();
            }

            return available;
        }
    }
}
