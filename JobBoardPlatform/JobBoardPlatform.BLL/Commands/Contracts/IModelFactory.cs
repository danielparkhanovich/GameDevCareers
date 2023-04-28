using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Models.Contracts
{
    public interface IModelFactory<TData, TEntity>
        where TData : class
        where TEntity : IEntity
    {
        TEntity CreateModel(TData data);
    }
}
