using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class AddNewUserCommand<T> : ICommand
        where T : class, IUserIdentityEntity
    {
        private readonly IRepository<T> repository;
        private readonly T identity;

        public T AddedRecord { get; private set; }


        public AddNewUserCommand(IRepository<T> repository, T identity)
        {
            this.repository = repository;
            this.identity = identity;
            this.AddedRecord = null;
        }

        public async Task Execute()
        {
            AddedRecord = await repository.Add(identity);
        }
    }
}
