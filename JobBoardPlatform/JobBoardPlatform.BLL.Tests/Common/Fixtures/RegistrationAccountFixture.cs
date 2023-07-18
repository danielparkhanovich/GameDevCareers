using Microsoft.Extensions.DependencyInjection;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.BLL.Services.Authentification.Register;
using JobBoardPlatform.BLL.Services.AccountManagement.Password;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class RegistrationAccountFixture : DbFixture
    {
        public RegistrationAccountFixture()
        {
            AddCommonRegistrationServices(serviceCollection);
            AddEmployeeRegistrationServices(serviceCollection);
            AddCompanyRegistrationServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddEmployeeRegistrationServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEmailEmployeeRegistrationService, EmailEmployeeRegistrationService>();


            serviceCollection.AddTransient<UserManager<EmployeeIdentity>>();
            serviceCollection.AddTransient<IdentityQueryExecutor<EmployeeIdentity>>();
            serviceCollection.AddTransient<IPasswordHasher, MD5Hasher>();
        }

        private void AddCompanyRegistrationServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<UserManager<CompanyIdentity>>();
            serviceCollection.AddTransient<IdentityQueryExecutor<CompanyIdentity>>();
            serviceCollection.AddTransient<IPasswordHasher, MD5Hasher>();
        }

        private void AddCommonRegistrationServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRegistrationTokensService, RegistrationTokensService>();
        }
    }
}
