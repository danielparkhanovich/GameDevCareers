using JobBoardPlatform.PL.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class AppFixture : DbFixture
    {
        public AppFixture()
        {
            var builder = WebApplication.CreateBuilder();
            serviceCollection.AddAuthenticationServices(builder.Configuration);
            serviceCollection.AddAuthorizationServices();
            serviceCollection.AddOffersServices();
            serviceCollection.AddPaymentServices(builder.Configuration);
            serviceCollection.AddActionsServices(builder.Environment);
            serviceCollection.AddAccountServices(builder.Configuration);
            serviceCollection.AddBackgroundServices();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
