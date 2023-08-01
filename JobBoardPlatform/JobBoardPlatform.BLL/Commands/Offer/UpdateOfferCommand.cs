using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.DAL.Managers;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class UpdateOfferCommand : ICommand
    {
        private readonly IOfferData data;
        private readonly OfferModelData offerModel;
        private readonly IMapper<IOfferData, JobOffer> dataToOffer;


        public UpdateOfferCommand(IOfferData data, OfferModelData offerModel)
        {
            this.data = data;
            this.offerModel = offerModel;
            this.dataToOffer = new JobOfferDataToEntityMapper();
        }

        public async Task Execute()
        {
            var offer = await offerModel.OffersRepository.Get(data.OfferId);
            await DeleteOldCollections(offer, data);
            dataToOffer.Map(data, offer);
            await offerModel.OffersRepository.Update(offer);
        }

        private async Task DeleteOldCollections(JobOffer offer, IOfferData newOffer)
        {
            if (newOffer.EmploymentTypes != null)
            {
                await offerModel.DeleteEmploymentDetails(offer);
            }

            if (newOffer.TechKeywords != null)
            {
                await offerModel.DeleteTechKeywords(offer);
            }
        }
    }
}
