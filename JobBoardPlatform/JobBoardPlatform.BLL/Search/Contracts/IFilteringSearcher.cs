using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Search.Contracts
{
    public interface IFilteringSearcher<TEntity, TSearchParams>
        where TEntity : class, IEntity
        where TSearchParams : IPageSearchParams
    {
        TSearchParams SearchParams { get; }
        Task<EntitiesFilteringSearchResponse<TEntity>> Search();
    }
}
