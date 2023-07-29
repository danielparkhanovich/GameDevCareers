using Microsoft.Extensions.DependencyInjection;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.IntegrationTests.Common.Mocks.Services;
using JobBoardPlatform.DAL.Models.Employee;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class OffersManagementFixture : DbFixture
    {
        public OffersManagementFixture()
        {
            AddRegistrationServices(serviceCollection);
            AddOffersManagementServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddRegistrationServices(ServiceCollection serviceCollection)
        {
            AddEmployeeRegistrationServices(serviceCollection);
            AddCompanyRegistrationServices(serviceCollection);
            serviceCollection.AddTransient<IPasswordHasher, MD5Hasher>();
        }

        private void AddEmployeeRegistrationServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<UserManager<EmployeeIdentity>>();
            serviceCollection.AddTransient<IdentityQueryExecutor<EmployeeIdentity>>();
        }

        private void AddCompanyRegistrationServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<UserManager<CompanyIdentity>>();
            serviceCollection.AddTransient<IdentityQueryExecutor<CompanyIdentity>>();
        }

        private void AddOffersManagementServices(ServiceCollection serviceCollection) 
        {
            serviceCollection.AddTransient<IOfferQueryExecutor, OfferQueryExecutorMock>();
            serviceCollection.AddTransient<IOffersManager, OffersManager>();
            serviceCollection.AddTransient<IOffersCacheManager, OffersCacheManagerMock>();
            serviceCollection.AddTransient<MainPageOffersSearcher>();
        }
    }
}
