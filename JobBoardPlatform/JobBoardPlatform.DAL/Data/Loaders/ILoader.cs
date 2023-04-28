using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public interface ILoader<T> where T : class
    {
        Task<T> Load();
    }
}
