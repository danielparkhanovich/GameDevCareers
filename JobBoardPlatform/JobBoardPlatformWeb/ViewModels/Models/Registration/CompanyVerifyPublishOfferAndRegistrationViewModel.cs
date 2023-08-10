using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Models.Registration
{
    public class CompanyVerifyPublishOfferAndRegistrationViewModel
    {
        public bool IsTokenConfirmed { get; set; }
        public string FormDataTokenId { get; set; }
        public OfferPaymentFormViewModel PaymentForm { get; set; } = new OfferPaymentFormViewModel();
        public CompanyRegisterViewModel UserRegister { get; set; } = new CompanyRegisterViewModel();
    }
}
