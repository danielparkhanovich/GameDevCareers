using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Admin;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    internal class DeleteAdminCommand : ICommand
    {
        private readonly IRepository<AdminIdentity> repository;
        private readonly int idToDelete;


        public DeleteAdminCommand(
            IRepository<AdminIdentity> repository,
            int idToDelete)
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
