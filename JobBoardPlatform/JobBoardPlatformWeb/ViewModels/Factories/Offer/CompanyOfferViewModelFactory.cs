using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class CompanyOfferViewModelFactory : IFactory<CompanyOfferCardViewModel>
    {
        private readonly JobOffer offer;
        private readonly OfferStateFactory offerStateFactory;


        public CompanyOfferViewModelFactory(JobOffer offer)
        {
            this.offer = offer;
            this.offerStateFactory = new OfferStateFactory();
        }

        public async Task<CompanyOfferCardViewModel> Create()
        {
            var offerCard = new CompanyOfferCardViewModel();

            offerCard.IsVisible = offerStateFactory.IsOfferVisible(offer);
            offerCard.IsAvailable = offerStateFactory.IsOfferAvailable(offer);
            offerCard.StateType = offerStateFactory.GetOfferState(offer);

            Map(offer, offerCard);
            await MapCardDisplay(offer, offerCard);

            return offerCard;
        }

        private void Map(JobOffer from, CompanyOfferCardViewModel to)
        {
            to.TotalViews = from.NumberOfViews;
            to.TotalApplicants = from.NumberOfApplications;
            to.MainTechnology = from.MainTechnologyType.Type;
            to.ContactType = from.ContactDetails.ContactType.Type;
            to.ContactAddress = from.ContactDetails.ContactAddress;

            to.IsPaid = from.IsPaid;
            to.IsPublished = from.IsPublished;
            to.IsShelved = from.IsShelved;
            to.IsDeleted = from.IsDeleted;
        }

        private async Task MapCardDisplay(JobOffer from, CompanyOfferCardViewModel to)
        {
            var offerCardFactory = new OfferCardViewModelFactory(from);
            var offerCardViewModel = await offerCardFactory.Create();

            to.CardDisplay = offerCardViewModel;
        }
    }
}
