using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.BLL.Search.Templates;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Offers
{
    public class CompanyOffersSearcher : FilteringPageSearcherBase<JobOffer, CompanyPanelOfferSearchParameters>
    {
        private readonly IRepository<JobOffer> repository;


        public CompanyOffersSearcher(
            IRepository<JobOffer> repository,
            IPageSearchParamsFactory<CompanyPanelOfferSearchParameters> paramsFactory)
            : base(paramsFactory)
        {
            this.repository = repository;
        }

        protected override IRepository<JobOffer> GetRepository()
        {
            return repository;
        }

        protected override IQueryable<JobOffer> GetFiltered(IQueryable<JobOffer> available)
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

        protected override IQueryable<JobOffer> GetSorted(IQueryable<JobOffer> available)
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

        protected override IEntityLoader<JobOffer> GetLoader()
        {
            return new OfferQueryLoader();
        }
    }
}
