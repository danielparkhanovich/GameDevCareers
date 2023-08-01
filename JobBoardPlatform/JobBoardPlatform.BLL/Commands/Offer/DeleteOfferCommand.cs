using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Managers;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteOfferCommand : ICommand
    {
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly OfferModelData offerModel;
        private readonly int offerIdToDelete;


        public DeleteOfferCommand(
            IOfferQueryExecutor queryExecutor,
            OfferModelData offerModel,
            int offerIdToDelete)
        {
            this.queryExecutor = queryExecutor;
            this.offerModel = offerModel;
            this.offerIdToDelete = offerIdToDelete;
        }

        public async Task Execute()
        {
            var offer = await queryExecutor.GetOfferById(offerIdToDelete);

            await UnassignResumesFromOffer(offer);
            await offerModel.OffersRepository.Delete(offerIdToDelete);
            await DeleteOfferContent(offer);
        }

        private async Task DeleteOfferContent(JobOffer offer)
        {
            await offerModel.DeleteEmploymentDetails(offer);
            await offerModel.DeleteTechKeywords(offer);
            await offerModel.ContactDetailsRepository.Delete(offer.ContactDetailsId);
        }

        private async Task UnassignResumesFromOffer(JobOffer offer)
        {
            if (offer.OfferApplications == null)
            {
                return;
            }

            foreach (var application in offer.OfferApplications)
            {
                var deleteApplicationCommand = new DeleteApplicationCommand(application.Id, offerModel);
                await deleteApplicationCommand.Execute();
            }
        }
    }
}
