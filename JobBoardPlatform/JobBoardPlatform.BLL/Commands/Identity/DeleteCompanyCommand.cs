using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteCompanyCommand : ICommand
    {
        private readonly IRepository<CompanyIdentity> repository;
        private readonly int idToDelete;


        public DeleteCompanyCommand(IRepository<CompanyIdentity> repository, int idToDelete)
        {
            this.repository = repository;
            this.idToDelete = idToDelete;
        }

        public async Task Execute()
        {
            await repository.Delete(idToDelete);
        }
    }
}
