using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.DAL.Data.Enums
{
    public interface IEnumModelCreator
    {
        public void SetDataForEntity<TEntity, TEnum>()
            where TEntity : class, IEnumEntity, new()
            where TEnum : struct;
    }
}
