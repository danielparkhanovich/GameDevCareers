using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminOfferViewModelFactory : IContainerCardFactory<JobOffer>
    {
        public IContainerCard CreateCard(JobOffer offer)
        {
            var adminCard = new AdminOfferCardViewModel();

            adminCard.CardViewModel = GetOfferCard(offer);
            adminCard.IsSuspended = offer.IsSuspended;

            return adminCard;
        }

        private CompanyOfferCardViewModel GetOfferCard(JobOffer offer)
        {
            var cardFactory = new CompanyOfferViewModelFactory();
            var companyCard = (cardFactory.CreateCard(offer) as CompanyOfferCardViewModel)!;
            return companyCard;
        }
    }
}
