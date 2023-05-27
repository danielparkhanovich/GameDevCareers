using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Utils;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    public class GenerateCompaniesCommand : ICommand
    {
        private readonly int countToGenerate;
        private readonly IRepository<CompanyIdentity> identityRepository;


        public GenerateCompaniesCommand(int countToGenerate,
            IRepository<CompanyIdentity> identityRepository)
        {
            this.countToGenerate = countToGenerate;
            this.identityRepository = identityRepository;
        }

        public async Task Execute()
        {
            var generator = new CompaniesGenerator();
            for (int i = 0; i < countToGenerate; i++)
            {
                await AddNew(generator);
            }
        }

        private async Task AddNew(CompaniesGenerator generator)
        {
            var data = generator.GenerateData() as CompanyIdentity;
            var addNewUserCommand = new AddNewUserCommand<CompanyIdentity>(identityRepository, data);
            await addNewUserCommand.Execute();
        }
    }
}
