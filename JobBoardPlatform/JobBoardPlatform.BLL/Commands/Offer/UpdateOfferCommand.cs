using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.DAL.Contexts;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class UpdateOfferCommand : ICommand
    {
        private readonly OfferData data;
        private readonly IOfferManager offerManager;
        private readonly IMapper<OfferData, JobOffer> dataToOffer;
        private readonly OfferContext offerModel;


        public UpdateOfferCommand(
            OfferData data,
            IOfferManager offerManager,
            OfferContext offerModel)
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
            MapData(offer);
            await offerModel.OffersRepository.Update(offer);
        }

        private async Task DeleteOldCollections(JobOffer offer, OfferData newOffer)
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

        private void MapData(JobOffer offer)
        {
            if (offer.IsPaid)
            {
                data.PlanId = offer.PlanId;
            }
            dataToOffer.Map(data, offer);
        }

        private void SortTechKeywords(OfferData data)
        {
            if (data.TechKeywords != null)
            {
                data.TechKeywords = data.TechKeywords.OrderBy(x => x).ToArray();
            }
        }
    }
}
