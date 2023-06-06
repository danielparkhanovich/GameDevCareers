using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.BLL.Search.Templates;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Applications
{
    public class OfferApplicationsSearcher : FilteringPageSearcherBase<OfferApplication, CompanyPanelApplicationSearchParams>
    {
        private readonly IRepository<OfferApplication> repository;


        public OfferApplicationsSearcher(IRepository<OfferApplication> repository)
        {
            this.repository = repository;
        }

        protected override IRepository<OfferApplication> GetRepository()
        {
            return repository;
        }

        protected override IQueryable<OfferApplication> GetFiltered(IQueryable<OfferApplication> available)
        {
            available = available.Where(application => application.JobOfferId == searchParams.OfferId);

            available = available.Where(application =>
                 application.ApplicationFlagTypeId == 1 && searchParams.IsShowUnseen ||
                 application.ApplicationFlagTypeId == 2 && searchParams.IsShowMustHire ||
                 application.ApplicationFlagTypeId == 3 && searchParams.IsShowAverage ||
                 application.ApplicationFlagTypeId == 4 && searchParams.IsShowRejected);

            return available;
        }

        protected override IQueryable<OfferApplication> GetSorted(IQueryable<OfferApplication> available)
        {
            var category = searchParams.SortCategory;

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

            if (searchParams.Sort == SortType.Descending)
            {
                available = available.Reverse();
            }

            return available;
        }

        protected override IEntityLoader<OfferApplication> GetLoader()
        {
            return new ApplicationQueryLoader();
        }
    }
}
