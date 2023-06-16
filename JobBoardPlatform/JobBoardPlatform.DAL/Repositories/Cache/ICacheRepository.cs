namespace JobBoardPlatform.DAL.Repositories.Cache
{
    public interface ICacheRepository<T>
    {
        Task UpdateAsync(string entryKey, T entry);
        Task<T> GetAsync(string entryKey);
    }
}
