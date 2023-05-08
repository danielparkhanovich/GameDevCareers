using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteAllOffersCommand : ICommand
    {
        private readonly IRepository<JobOffer> offersRepository;


        public DeleteAllOffersCommand(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        public async Task Execute()
        {
            var allOffers = await offersRepository.GetAll();
            foreach (var offer in allOffers)
            {
                await offersRepository.Delete(offer.Id);
            }
        }
    }
}
