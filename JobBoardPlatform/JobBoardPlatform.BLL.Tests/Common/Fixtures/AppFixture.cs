using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.IntegrationTests.Common.Mocks.Services;
using JobBoardPlatform.PL.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class AppFixture : DbFixture
    {
        public AppFixture()
        {
            AddApplicationServices();
            AddMockServices();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddApplicationServices()
        {
            var builder = WebApplication.CreateBuilder();
            serviceCollection.AddAuthenticationServices(builder.Configuration);
            serviceCollection.AddAuthorizationServices();
            serviceCollection.AddOffersServices();
            serviceCollection.AddPaymentServices(builder.Configuration);
            serviceCollection.AddActionsServices(builder.Environment);
            serviceCollection.AddAccountServices(builder.Configuration);
            serviceCollection.AddBackgroundServices();
        }

        private void AddMockServices()
        {
            serviceCollection.AddTransient<IOfferCacheManager, OffersCacheManagerMock>();
            serviceCollection.AddTransient<IOfferQueryExecutor, OfferQueryExecutorMock>();
            serviceCollection.AddTransient<IEmailContent<JobOfferApplication>, EmailViewRendererMock>();
            serviceCollection.AddTransient<IEmailSender, EmailSenderMock>();
        }
    }
}
