using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class SuspendOfferCommand : ICommand
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly int offerId;
        private readonly bool flag;


        public SuspendOfferCommand(IRepository<JobOffer> offersRepository, int offerId, bool flag)
        {
            this.offersRepository = offersRepository;
            this.offerId = offerId;
            this.flag = flag;
        }

        public async Task Execute()
        {
            var offer = await offersRepository.Get(offerId);
            offer!.IsSuspended = flag;
            await offersRepository.Update(offer);
        }
    }
}
