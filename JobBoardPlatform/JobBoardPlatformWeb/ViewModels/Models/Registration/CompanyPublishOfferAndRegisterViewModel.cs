using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Company;

namespace JobBoardPlatform.PL.ViewModels.Models.Registration
{
    public class CompanyPublishOfferAndRegisterViewModel
    {
        public CompanyProfileViewModel CompanyRegistrationData { get; set; } = new CompanyProfileViewModel();
        public EditOfferViewModel EditOffer { get; set; } = new EditOfferViewModel();
    }
}
