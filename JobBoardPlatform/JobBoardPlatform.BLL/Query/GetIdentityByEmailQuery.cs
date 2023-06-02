
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Query
{
    internal class GetIdentityByEmailQuery<T> : IQuery<T>
        where T: class, IUserIdentityEntity
    {
        private readonly IRepository<T> repository;
        private readonly string email;


        public GetIdentityByEmailQuery(IRepository<T> repository, string email)
        {
            this.repository = repository;
            this.email = email;
        }

        public async Task<T> Get()
        {
            return await GetUserByEmailAsync(email);
        }

        private async Task<T> GetUserByEmailAsync(string email)
        {
            var userSet = await repository.GetAllSet();
            var user = await userSet.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }
    }
}
