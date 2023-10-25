using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class RedirectApplicationFormCommand : ICommand
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly int offerId;


        public RedirectApplicationFormCommand(IRepository<JobOffer> offersRepository, int offerId)
        {
            this.offersRepository = offersRepository;
            this.offerId = offerId;
        }

        public async Task Execute()
        {
            var offer = await offersRepository.Get(offerId);
            offer.NumberOfApplications += 1;
            await offersRepository.Update(offer);
        }
    }
}
