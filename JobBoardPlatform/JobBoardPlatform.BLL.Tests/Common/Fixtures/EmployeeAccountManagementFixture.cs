using Microsoft.Extensions.DependencyInjection;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Query.Identity;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class EmployeeAccountManagementFixture : DbFixture
    {
        public EmployeeAccountManagementFixture()
        {
            AddRegistrationServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddRegistrationServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<UserManager<EmployeeIdentity>>();
            serviceCollection.AddTransient<IdentityQueryExecutor<EmployeeIdentity>>();
            serviceCollection.AddTransient<IPasswordHasher, MD5Hasher>();
        }
    }
}
