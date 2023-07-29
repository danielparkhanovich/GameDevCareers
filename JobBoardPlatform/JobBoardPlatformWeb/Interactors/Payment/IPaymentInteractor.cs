
namespace JobBoardPlatform.PL.Interactors.Payment
{
    public interface IPaymentInteractor
    {
        public Task ProcessCheckout(int offerId);
    }
}
