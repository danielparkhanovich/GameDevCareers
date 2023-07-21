using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class AddNewOfferCommand : ICommand
    {
        private readonly int profileId;
        private readonly INewOfferData data;
        private readonly IMapper<INewOfferData, JobOffer> dataToOffer;
        private readonly IRepository<JobOffer> repository;


        public AddNewOfferCommand(int profileId,
            INewOfferData data,
            IRepository<JobOffer> repository)
        {
            this.profileId = profileId;
            this.data = data;
            this.repository = repository;
            this.dataToOffer = new NewOfferDataToJobOfferMapper();
        }

        public async Task Execute()
        {
            var offer = await repository.Get(data.OfferId);

            if (offer != null)
            {
                dataToOffer.Map(data, offer);
                await repository.Update(offer);
            }
            else
            {
                offer = new JobOffer();
                offer.CompanyProfileId = profileId;
                offer.CreatedAt = DateTime.Now;

                dataToOffer.Map(data, offer);
                await repository.Add(offer);
            }
        }
    }
}
