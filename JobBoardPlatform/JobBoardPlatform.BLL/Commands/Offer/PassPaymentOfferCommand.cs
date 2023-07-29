using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class PassPaymentOfferCommand : ICommand
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly int offerId;


        public PassPaymentOfferCommand(IRepository<JobOffer> offersRepository, int offerId)
        {
            this.offersRepository = offersRepository;
            this.offerId = offerId;
        }

        public async Task Execute()
        {
            var offer = await offersRepository.Get(offerId);
            offer.IsPaid = true;
            offer.IsPublished = true;
            offer.PublishedAt = DateTime.Now;
            await offersRepository.Update(offer);
        }
    }
}
