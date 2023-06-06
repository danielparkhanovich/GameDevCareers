using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Query
{
    internal class GetEntityByIdQuery<T> : IQuery<T> where T : class, IEntity
    {
        private readonly IRepository<T> repository;
        private readonly IEntityLoader<T> loader;
        private readonly int id;


        public GetEntityByIdQuery(IRepository<T> repository, IEntityLoader<T> loader, int id)
        {
            this.repository = repository;
            this.loader = loader;
            this.id = id;
        }

        public async Task<T> Get()
        {
            var set = await repository.GetAllSet();
            var loaded = loader.Load(set);
            return await set.SingleAsync(x => x.Id == id);
        }
    }
}
