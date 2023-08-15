using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class AddOfferCommand : ICommand
    {
        private readonly int profileId;
        private readonly IOfferData data;
        private readonly IMapper<IOfferData, JobOffer> dataToOffer;
        private readonly IRepository<JobOffer> repository;


        public AddOfferCommand(
            int profileId,
            IOfferData data,
            IRepository<JobOffer> repository)
        {
            this.profileId = profileId;
            this.data = data;
            this.repository = repository;
            this.dataToOffer = new JobOfferDataToEntityMapper();
        }

        public async Task Execute()
        {
            var offer = new JobOffer();
            offer.CompanyProfileId = profileId;
            offer.CreatedAt = DateTime.Now;
            offer.RefreshedOnPageAt = DateTime.Now;

            dataToOffer.Map(data, offer);
            await repository.Add(offer);
        }
    }
}
