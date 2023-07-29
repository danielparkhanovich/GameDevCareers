
namespace JobBoardPlatform.PL.Interactors.Payment
{
    public interface IPaymentInteractor
    {
        public Task ProcessCheckout(int offerId);
        public Task ConfirmCheckout(int offerId, string checkoutSessionId);
    }
}
