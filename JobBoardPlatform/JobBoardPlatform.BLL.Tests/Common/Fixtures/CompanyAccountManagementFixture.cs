using Microsoft.Extensions.DependencyInjection;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.IntegrationTests.Common.Mocks.Services;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class CompanyAccountManagementFixture : OffersManagementFixture
    {
        public CompanyAccountManagementFixture()
        {
            AddRegistrationServices(serviceCollection);
            AddOffersManagementServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddRegistrationServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<UserManager<CompanyIdentity>>();
            serviceCollection.AddTransient<IdentityQueryExecutor<CompanyIdentity>>();
            serviceCollection.AddTransient<IPasswordHasher, MD5Hasher>();
        }

        private void AddOffersManagementServices(ServiceCollection serviceCollection) 
        {
            serviceCollection.AddTransient<IOffersManager, OffersManager>();
            serviceCollection.AddTransient<IOffersCacheManager, OffersCacheManagerMock>();
            serviceCollection.AddTransient<MainPageOffersSearcher>();
        }
    }
}
