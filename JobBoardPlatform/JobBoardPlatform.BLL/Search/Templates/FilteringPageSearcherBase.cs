using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Search.Templates
{
    public abstract class FilteringPageSearcherBase<TEntity, TSearchParams> : IFilteringSearcher<TEntity, TSearchParams>
        where TEntity : class, IEntity
        where TSearchParams : class, IPageSearchParams, new()
    {
        protected TSearchParams searchParams;


        public async Task<EntitiesFilteringSearchResponse<TEntity>> Search(TSearchParams searchParams)
        {
            this.searchParams = searchParams;

            var initialRecords = await GetInitial();

            var filtered = GetFiltered(initialRecords);
            int totalRecordsAfterFilters = filtered.Count();

            var sorted = GetSorted(filtered);
            var onPage = GetRecordsOnPage(sorted);

            var entities = await GetLoadedList(onPage);

            return new EntitiesFilteringSearchResponse<TEntity>()
            {
                TotalRecordsAfterFilters = totalRecordsAfterFilters,
                Entities = entities
            };
        }

        protected abstract IRepository<TEntity> GetRepository();

        protected abstract IQueryable<TEntity> GetFiltered(IQueryable<TEntity> records);

        protected abstract IQueryable<TEntity> GetSorted(IQueryable<TEntity> records);

        protected abstract IEntityLoader<TEntity> GetLoader();

        private Task<DbSet<TEntity>> GetInitial()
        {
            var repository = GetRepository();
            return repository.GetAllSet();
        }

        private IQueryable<TEntity> GetRecordsOnPage(IQueryable<TEntity> records)
        {
            int recordsFromStart = (searchParams.Page - 1) * searchParams.PageSize;
            return records.Skip(recordsFromStart)
                          .Take(searchParams.PageSize);
        }

        private Task<List<TEntity>> GetLoadedList(IQueryable<TEntity> records)
        {
            var loader = GetLoader();
            var loaded = loader.Load(records);
            return loaded.ToListAsync();
        }
    }
}
