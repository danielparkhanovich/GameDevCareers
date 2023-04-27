namespace JobBoardPlatform.BLL.Services.Payment.Gateway
{
    public class PaymentGateway : IPaymentGateway
    {
        // Use stripe
        // https://stackoverflow.com/questions/62749790/how-to-integrate-stripe-payment-gateway-to-asp-net-mvc
        public Task SendPaymentRequest()
        {
            throw new NotImplementedException();
        }
    }
}
