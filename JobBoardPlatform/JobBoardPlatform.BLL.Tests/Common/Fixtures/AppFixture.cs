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
            ServiceProvider = services.BuildServiceProvider();
        }

        private void AddApplicationServices()
        {
            var builder = WebApplication.CreateBuilder();
            services.AddAuthenticationServices(builder.Configuration);
            services.AddAuthorizationServices();
            services.AddOffersServices();
            services.AddPaymentServices(builder.Configuration);
            services.AddActionsServices(builder.Environment);
            services.AddAccountServices(builder.Configuration);
            services.AddBackgroundServices(builder.Environment);
        }

        private void AddMockServices()
        {
            TestSetup.AddMockServices(services);
        }
    }
}
