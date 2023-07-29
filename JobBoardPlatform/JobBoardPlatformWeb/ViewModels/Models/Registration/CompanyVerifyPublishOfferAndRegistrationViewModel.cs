using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Models.Registration
{
    public class CompanyVerifyPublishOfferAndRegistrationViewModel
    {
        public bool IsTokenConfirmed { get; set; }
        public string FormDataTokenId { get; set; }
        public IContainerCard OfferCard { get; set; } = new OfferCardViewModel();
        public CompanyRegisterViewModel UserRegister { get; set; } = new CompanyRegisterViewModel();
    }
}
