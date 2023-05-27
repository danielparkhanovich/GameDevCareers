using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteAllEmployeesCommand : ICommand
    {
        private readonly IRepository<EmployeeIdentity> repository;


        public DeleteAllEmployeesCommand(IRepository<EmployeeIdentity> repository)
        {
            this.repository = repository;
        }

        public async Task Execute()
        {
            var allRecords = await repository.GetAll();
            foreach (var offer in allRecords)
            {
                await Delete(offer.Id);
            }
        }

        private async Task Delete(int id)
        {
            var deleteCommand = new DeleteEmployeeCommand(repository, id);
            await deleteCommand.Execute();
        }
    }
}
