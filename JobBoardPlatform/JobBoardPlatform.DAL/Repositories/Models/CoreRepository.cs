using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Repositories.Models
{
    public class CoreRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DataContext context;


        public CoreRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<DbSet<TEntity>> GetAllSet()
        {
            return await Task.FromResult(context.Set<TEntity>());
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}
