namespace JobBoardPlatform.BLL.Services.Payment.Gateway
{
    public interface IPaymentGateway
    {
        public Task SendPaymentRequest();
    }
}
