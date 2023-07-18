using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Company;

namespace JobBoardPlatform.PL.ViewModels.Models.Registration
{
    public class CompanyPublishOfferAndRegisterViewModel : ICompanyProfileAndNewOfferData
    {
        public ICompanyProfileData CompanyProfileData { get; set; } = new CompanyProfileViewModel();

        public INewOfferData OfferData { get => EditOffer.OfferDetails; set => EditOffer.OfferDetails = value; }

        public EditOfferViewModel EditOffer { get; set; } = new EditOfferViewModel();
    }
}
