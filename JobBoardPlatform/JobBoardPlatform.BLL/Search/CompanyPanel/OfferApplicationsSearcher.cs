using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Search.CompanyPanel
{
    public class OfferApplicationsSearcher : ISearcher<OfferApplication, CompanyPanelApplicationSearchParameters>
    {
        public CompanyPanelApplicationSearchParameters SearchParams { get; set; }
        public int AfterFiltersCount { get; set; }


        public OfferApplicationsSearcher(CompanyPanelApplicationSearchParameters searchData)
        {
            this.SearchParams = searchData;
            AfterFiltersCount = 0;
        }

        public async Task<List<OfferApplication>> Search(IRepository<OfferApplication> repository)
        {
            var applicationsSet = await repository.GetAllSet();

            var filtered = GetFiltered(applicationsSet);
            AfterFiltersCount = filtered.Count();

            var sorted = GetSorted(filtered);

            int page = SearchParams.Page;
            int pageSize = SearchParams.PageSize;

            var pageOffers = sorted.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var loader = new LoadApplicationsPage(pageOffers);
            var loaded = await loader.Load();

            return loaded;
        }

        private IQueryable<OfferApplication> GetFiltered(IQueryable<OfferApplication> available)
        {
            available = available.Where(application => application.JobOfferId == SearchParams.OfferId);

            available = available.Where(application =>
                 application.ApplicationFlagTypeId == 1 && SearchParams.IsShowUnseen ||
                 application.ApplicationFlagTypeId == 2 && SearchParams.IsShowMustHire ||
                 application.ApplicationFlagTypeId == 3 && SearchParams.IsShowAverage ||
                 application.ApplicationFlagTypeId == 4 && SearchParams.IsShowRejected);

            return available;
        }

        private IQueryable<OfferApplication> GetSorted(IQueryable<OfferApplication> available)
        {
            var category = SearchParams.SortCategory;

            if (category == SortCategoryType.PublishDate.ToString())
            {
                available = available.OrderBy(x => x.CreatedAt);
            }
            else if (category == SortCategoryType.Alphabetically.ToString())
            {
                available = available.OrderBy(x => x.FullName);
            }
            else if (category == SortCategoryType.Relevenacy.ToString())
            {
                available = available.OrderBy(x =>
                    x.ApplicationFlagTypeId == 1 ? 1 :
                    x.ApplicationFlagTypeId == 2 ? 3 :
                    x.ApplicationFlagTypeId == 3 ? 2 :
                    0);
            }

            if (SearchParams.Sort == SortType.Descending)
            {
                available = available.Reverse();
            }

            return available;
        }
    }
}
