using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteAllOffersCommand : ICommand
    {
        private readonly IRepository<JobOffer> repository;


        public DeleteAllOffersCommand(IRepository<JobOffer> offersRepository)
        {
            this.repository = offersRepository;
        }

        public async Task Execute()
        {
            var allRecords = await repository.GetAll();
            foreach (var record in allRecords)
            {
                await DeleteOffer(record.Id);
            }
        }

        private async Task DeleteOffer(int id)
        {
            var deleteCommand = new DeleteOfferCommand(repository, id);
            await deleteCommand.Execute();
        }
    }
}
