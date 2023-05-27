using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteEmployeeCommand : ICommand
    {
        private readonly IRepository<EmployeeIdentity> repository;
        private readonly int idToDelete;


        public DeleteEmployeeCommand(IRepository<EmployeeIdentity> repository, int idToDelete)
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
