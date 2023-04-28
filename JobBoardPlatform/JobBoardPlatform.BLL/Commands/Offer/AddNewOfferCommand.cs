using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.BLL.Models.Contracts;
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
            IRepository<TechKeyword> keywordsRepository,
            IRepository<JobOffer> repository)
        {
            this.profileId = profileId;
            this.data = data;
            this.repository = repository;
            this.dataToOffer = new NewOfferDataToJobOfferMapper(keywordsRepository);
        }

        public async Task Execute()
        {
            var offer = new JobOffer();
            offer.CompanyProfileId = profileId;
            offer.CreatedAt = DateTime.Now;

            dataToOffer.Map(data, offer);

            await repository.Add(offer);
        }
    }
}
