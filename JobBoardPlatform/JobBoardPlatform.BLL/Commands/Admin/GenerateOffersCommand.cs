using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Utils;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    public class GenerateOffersCommand : ICommand
    {
        private readonly int offersCount;
        private readonly int companyId;
        private readonly IRepository<CompanyIdentity> repository;
        private readonly IRepository<JobOffer> offersRepository;


        public GenerateOffersCommand(int offersCount,
            int companyId, 
            IRepository<CompanyIdentity> repository,
            IRepository<JobOffer> offersRepository)
        {
            this.offersCount = offersCount;
            this.companyId = companyId;
            this.repository = repository;
            this.offersRepository = offersRepository;
        }

        public async Task Execute()
        {
            var toProcess = new List<CompanyIdentity>();
            var company = await repository.Get(companyId); 

            if (company != null)
            {
                toProcess.Add(company);
            }
            else
            {
                toProcess = await repository.GetAll();
            }

            var offersGenerator = new JobOffersGenerator();

            foreach (var companyIdentity in toProcess)
            {
                for (int i = 0; i < offersCount; i++)
                {
                    var data = offersGenerator.GenerateData(companyIdentity);
                    var addNewOfferCommand = new AddOfferCommand(companyIdentity.ProfileId,
                        data,
                        offersRepository);
                    await addNewOfferCommand.Execute();
                }
            }
        }
    }
}
