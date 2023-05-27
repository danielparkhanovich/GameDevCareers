using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Utils;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    public class GenerateEmployeesCommand : ICommand
    {
        private readonly int countToGenerate;
        private readonly IRepository<EmployeeIdentity> identityRepository;


        public GenerateEmployeesCommand(int countToGenerate,
            IRepository<EmployeeIdentity> identityRepository)
        {
            this.countToGenerate = countToGenerate;
            this.identityRepository = identityRepository;
        }

        public async Task Execute()
        {
            var generator = new EmployeesGenerator();
            for (int i = 0; i < countToGenerate; i++)
            {
                await AddNew(generator);
            }
        }

        private async Task AddNew(EmployeesGenerator generator)
        {
            var data = generator.GenerateData() as EmployeeIdentity;
            var addNewUserCommand = new AddNewUserCommand<EmployeeIdentity>(identityRepository, data);
            await addNewUserCommand.Execute();
        }
    }
}
