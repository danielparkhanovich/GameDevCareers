using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteOfferCommand : ICommand
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly int offerIdToDelete;


        public DeleteOfferCommand(IRepository<JobOffer> offersRepository, int offerIdToDelete)
        {
            this.offersRepository = offersRepository;
            this.offerIdToDelete = offerIdToDelete;
        }

        public async Task Execute()
        {
            var offer = await offersRepository.Get(offerIdToDelete);
            offer!.IsDeleted = true;
            await offersRepository.Update(offer);
            // await offersRepository.Delete(offerIdToDelete);
        }
    }
}
