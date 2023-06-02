using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public interface IEntityLoader<T> where T : class, IEntity
    {
        IQueryable<T> Load(IQueryable<T> entities);
    }
}
