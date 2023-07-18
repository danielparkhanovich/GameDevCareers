using Microsoft.Extensions.DependencyInjection;
using JobBoardPlatform.BLL.Services.AccountManagement.Password;
using JobBoardPlatform.BLL.Services.AccountManagement.Password.Tokens;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Email;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Login;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Commands.Identity;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class AccountOptionsFixture : DbFixture
    {
        public AccountOptionsFixture()
        {
            AddResetPasswordServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void AddResetPasswordServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEmailSender, EmailSender>();
            serviceCollection.AddTransient<IRestorePasswordTokensService, RestorePasswordTokensService>();
            //serviceCollection.AddScoped<IConfirmationLinkFactory, ConfirmationLinkFactory>();
            serviceCollection.AddTransient<UserManager<EmployeeIdentity>>();
            serviceCollection.AddTransient<UserManager<CompanyIdentity>>();
            serviceCollection.AddTransient(typeof(IModifyIdentityService<>), typeof(ModifyIdentityService<>));
            serviceCollection.AddTransient(typeof(IAuthorizationService<,>), typeof(AuthorizationService<,>));
            serviceCollection.AddTransient<IdentityQueryExecutor<EmployeeIdentity>>();
            serviceCollection.AddTransient<IdentityQueryExecutor<CompanyIdentity>>();

            serviceCollection.AddTransient(typeof(IResetPasswordService<,>), typeof(RestorePasswordService<,>));
            serviceCollection.AddTransient<IRestorePasswordTokensService, RestorePasswordTokensService>();
        }

        private void AddOptionsConfigurationServices()
        {
            //builder.Services.AddTransient<IdentityQueryExecutor<EmployeeIdentity>>();
            //builder.Services.AddTransient<IdentityQueryExecutor<CompanyIdentity>>();
        }
    }
}
