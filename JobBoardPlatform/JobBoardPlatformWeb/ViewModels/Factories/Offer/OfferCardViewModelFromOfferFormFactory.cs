using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.BLL.Common.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    internal class OfferCardViewModelFromOfferFormFactory : IFactory<IContainerCard>
    {
        private readonly IOfferData offerData;
        private readonly ICompanyProfileData profileData;


        public OfferCardViewModelFromOfferFormFactory(IOfferData offerData, ICompanyProfileData profileData)
        {
            this.offerData = offerData;
            this.profileData = profileData;
        }

        public IContainerCard Create()
        {
            var offer = new JobOffer();
            MapCompanyData(profileData, offer);
            MapOfferData(offerData, offer);

            // TODO: fix
            offer.WorkLocation = new DAL.Models.EnumTables.WorkLocationType();
            offer.TechKeywords = new List<JobOfferTechKeyword>();

            var offerCardViewModelFactory = new OfferCardViewModelFactory();
            return offerCardViewModelFactory.CreateCard(offer);
        }

        private void MapOfferData(IOfferData from, JobOffer to)
        {
            var mapper = new JobOfferDataToEntityMapper();
            mapper.Map(from, to);
        }

        private void MapCompanyData(ICompanyProfileData from, JobOffer to)
        {
            to.CompanyProfile = new CompanyProfile()
            {
                City = from.OfficeCity,
                Country = from.OfficeCountry,
                CompanyName = from.CompanyName,
                CompanyWebsiteUrl = from.CompanyWebsiteUrl
            };
        }
    }
}
