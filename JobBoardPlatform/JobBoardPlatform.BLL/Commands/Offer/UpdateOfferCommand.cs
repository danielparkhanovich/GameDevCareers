using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.DAL.Managers;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class UpdateOfferCommand : ICommand
    {
        private readonly IOfferData data;
        private readonly IOfferManager offerManager;
        private readonly IMapper<IOfferData, JobOffer> dataToOffer;
        private readonly OfferModelData offerModel;


        public UpdateOfferCommand(
            IOfferData data,
            IOfferManager offerManager,
            OfferModelData offerModel)
        {
            this.data = data;
            this.offerManager = offerManager;
            this.offerModel = offerModel;
            this.dataToOffer = new JobOfferDataToEntityMapper();
        }

        public async Task Execute()
        {
            var offer = await offerManager.GetAsync(data.OfferId);
            await DeleteOldCollections(offer, data);
            SortTechKeywords(data);
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

        private void SortTechKeywords(IOfferData data)
        {
            if (data.TechKeywords != null)
            {
                data.TechKeywords = data.TechKeywords.OrderBy(x => x).ToArray();
            }
        }
    }
}
