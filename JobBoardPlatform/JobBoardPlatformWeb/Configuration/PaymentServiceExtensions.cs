
using JobBoardPlatform.PL.Interactors.Payment;
using Stripe;

namespace JobBoardPlatform.PL.Configuration
{
    public static class PaymentServiceExtensions
    {
        public static void AddPaymentServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            AddPaymentGateway(services, configuration);
            AddServices(services);
        }

        private static void AddPaymentGateway(IServiceCollection services, ConfigurationManager configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetValue<string>("StripeGateway:SecretKey");
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IPaymentInteractor, StripePaymentInteractor>();
        }
    }
}
