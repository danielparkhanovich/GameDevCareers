using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteAllCompaniesCommand : ICommand
    {
        private readonly IRepository<CompanyIdentity> repository;


        public DeleteAllCompaniesCommand(IRepository<CompanyIdentity> repository)
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
            var deleteCommand = new DeleteCompanyCommand(repository, id);
            await deleteCommand.Execute();
        }
    }
}
