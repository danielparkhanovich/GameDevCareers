using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteAllOffersCommand : ICommand
    {
        private readonly IRepository<JobOffer> repository;
        private readonly IOfferManager offersManager;


        public DeleteAllOffersCommand(
            IRepository<JobOffer> offersRepository,
            IOfferManager offersManager)
        {
            this.repository = offersRepository;
            this.offersManager = offersManager;
        }

        public async Task Execute()
        {
            var allRecords = await repository.GetAll();
            foreach (var record in allRecords)
            {
                await offersManager.DeleteAsync(record.Id);
            }
        }
    }
}
