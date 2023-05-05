using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Search.Offers
{
    public class SearchActualOffers : ISearcher<List<JobOffer>>
    {
        private readonly IRepository<JobOffer> repository;
        private readonly OfferSearchDataUrlFactory searchDataFactory;
        private readonly int pageSize;

        public int AfterFiltersCount { get; set; }


        public SearchActualOffers(IRepository<JobOffer> repository, HttpRequest request, int pageSize) 
        {
            this.repository = repository;
            this.searchDataFactory = new OfferSearchDataUrlFactory(request);
            this.pageSize = pageSize;
        }

        public async Task<List<JobOffer>> Search()
        {
            var offersSet = await repository.GetAllSet();
            var available = offersSet.Where(offer =>
                 !offer.IsDeleted &&
                 offer.IsPaid &&
                 !offer.IsSuspended &&
                 !offer.IsShelved &&
                 offer.IsPublished);

            var searchData = searchDataFactory.Create();
            var filtered = GetFiltered(searchData, available);
            AfterFiltersCount = filtered.Count();
            var sorted = GetSorted(searchData, available);

            int page = searchData.Page;
            var pageOffers = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var loader = new LoadActualOffersPage(filtered);
            var loaded = await loader.Load();

            return loaded;
        }

        private IQueryable<JobOffer> GetFiltered(OfferSearchData searchData, IQueryable<JobOffer> available) 
        {
            if (searchData.IsRemoteOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.WorkLocation)
                    .Where(offer => offer.WorkLocation.Type == fullyRemoteString);
            }
            if (searchData.IsSalaryOnly)
            {
                string fullyRemoteString = WorkLocationTypeEnum.FullyRemote.ToString();

                available = available.Include(offer => offer.JobOfferEmploymentDetails)
                    .ThenInclude(details => details.SalaryRange)
                    .Where(offer => offer.JobOfferEmploymentDetails.Any(x => x.SalaryRange != null));
            }
            if (searchData.MainTechnology != 0)
            {
                available = available.Where(offer => offer.MainTechnologyTypeId == searchData.MainTechnology);
            }
            if (!string.IsNullOrEmpty(searchData.SearchString))
            {
                // search logic
            }

            return available;
        }

        private IQueryable<JobOffer> GetSorted(OfferSearchData searchData, IQueryable<JobOffer> available)
        {
            available = available.OrderByDescending(offer => offer.PublishedAt);
            return available;
        }
    }
}
