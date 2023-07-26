using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.BLL.Search.Templates;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Applications
{
    public class OfferApplicationsSearcher : FilteringPageSearcherBase<JobOfferApplication, CompanyPanelApplicationSearchParams>
    {
        private readonly IRepository<JobOfferApplication> repository;


        public OfferApplicationsSearcher(IRepository<JobOfferApplication> repository)
        {
            this.repository = repository;
        }

        protected override IRepository<JobOfferApplication> GetRepository()
        {
            return repository;
        }

        protected override IQueryable<JobOfferApplication> GetFiltered(IQueryable<JobOfferApplication> available)
        {
            available = available.Where(application => application.JobOfferId == searchParams.OfferId);

            available = available.Where(application =>
                 application.ApplicationFlagTypeId == 1 && searchParams.IsShowUnseen ||
                 application.ApplicationFlagTypeId == 2 && searchParams.IsShowMustHire ||
                 application.ApplicationFlagTypeId == 3 && searchParams.IsShowAverage ||
                 application.ApplicationFlagTypeId == 4 && searchParams.IsShowRejected);

            return available;
        }

        protected override IQueryable<JobOfferApplication> GetSorted(IQueryable<JobOfferApplication> available)
        {
            var category = searchParams.SortCategory;

            if (category == SortCategoryType.PublishDate.ToString())
            {
                available = available.OrderByDescending(x => x.CreatedAt);
            }
            else if (category == SortCategoryType.Alphabetically.ToString())
            {
                available = available.OrderBy(x => x.FullName);
            }
            else if (category == SortCategoryType.Relevenacy.ToString())
            {
                available = available.OrderByDescending(x =>
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

        protected override IEntityLoader<JobOfferApplication> GetLoader()
        {
            return new ApplicationQueryLoader();
        }
    }
}
