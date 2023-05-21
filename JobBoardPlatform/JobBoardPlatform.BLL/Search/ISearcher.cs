using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Search
{
    public interface ISearcher<TEntity, TSearch> 
        where TEntity : class, IEntity
        where TSearch : ISearchParameters
    {
        TSearch SearchParams { get; }
        int AfterFiltersCount { get; protected set; }

        Task<List<TEntity>> Search(IRepository<TEntity> repository);
    }
}
