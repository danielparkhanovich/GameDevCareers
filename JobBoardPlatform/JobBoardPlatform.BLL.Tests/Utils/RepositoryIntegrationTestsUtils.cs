using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.AccountManagement.Common;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Utils
{
    public class RepositoryIntegrationTestsUtils
    {
        private readonly IServiceProvider serviceProvider;
        private readonly UserManager<EmployeeIdentity> userManager;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRepository<EmployeeIdentity> employeeRepository;


        public RepositoryIntegrationTestsUtils(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.userManager = serviceProvider.GetService<UserManager<EmployeeIdentity>>()!;
            this.passwordHasher = serviceProvider.GetService<IPasswordHasher>()!;
            this.employeeRepository = serviceProvider.GetService<IRepository<EmployeeIdentity>>()!;
        }

        public Task AddExampleEmployeeToRepositoryAsync(string email)
        {
            return AddExampleEmployeeToRepositoryAsync(email, "1234567890");
        }

        public Task AddExampleEmployeeToRepositoryAsync(string email, string password)
        {
            string passwordHash = passwordHasher.GetHash(password);
            var employee = new EmployeeIdentity()
            {
                Email = email,
                HashPassword = passwordHash,
                Profile = new EmployeeProfile()
            };

            return userManager.AddNewUser(employee);
        }

        public Task<EmployeeIdentity> GetEmployeeByEmail(string email)
        {
            return userManager.GetUserByEmail(email);
        }

        public async Task<DeleteEmployeeCommand> GetDeleteEmployeeCommandByEmail(string email)
        {
            var user = await userManager.GetUserByEmail(email);
            var deleteCommand = new DeleteEmployeeCommand(employeeRepository, user.Id);
            return deleteCommand;
        }
    }
}
