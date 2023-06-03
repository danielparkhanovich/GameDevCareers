using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Search.Contracts
{
    public interface IFilteringSearcher<TEntity, TSearchParams>
        where TEntity : class, IEntity
        where TSearchParams : IPageSearchParams
    {
        Task<EntitiesFilteringSearchResponse<TEntity>> Search(TSearchParams searchParams);
    }
}
