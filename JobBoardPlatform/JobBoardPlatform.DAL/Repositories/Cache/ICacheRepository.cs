namespace JobBoardPlatform.DAL.Repositories.Cache
{
    public interface ICacheRepository<T>
    {
        Task UpdateAsync(T entry);
        Task<T> GetAsync();
    }
}
