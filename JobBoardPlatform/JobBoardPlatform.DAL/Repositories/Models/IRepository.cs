using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Repositories.Models
{
    public interface IRepository<T> where T : class, IEntity
    {
        public Task<List<T>> GetAll();
        public Task<DbSet<T>> GetAllSet();
        public Task<T> Get(int id);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(int id);
    }
}
