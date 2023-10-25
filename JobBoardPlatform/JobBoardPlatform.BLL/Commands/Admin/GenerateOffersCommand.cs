using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Identity;
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
        private readonly UserManager<CompanyIdentity> companyManager;
        private readonly IRepository<JobOffer> offersRepository;


        public GenerateOffersCommand(int offersCount,
            int companyId, 
            UserManager<CompanyIdentity> companyManager,
            IRepository<JobOffer> offersRepository)
        {
            this.offersCount = offersCount;
            this.companyId = companyId;
            this.companyManager = companyManager;
            this.offersRepository = offersRepository;
        }

        public async Task Execute()
        {
            var toProcess = new List<CompanyIdentity>();
            var company = await companyManager.GetAsync(companyId); 

            if (company != null)
            {
                toProcess.Add(company);
            }
            else
            {
                toProcess = await companyManager.GetAllAsync();
            }

            var offersGenerator = new JobOffersGenerator();
            var generatedData = GenerateOffersData(offersGenerator, toProcess);
            ShuffleOffers(generatedData);
            await CreateOffers(generatedData);
        }

        private List<(CompanyIdentity, IOfferData)> GenerateOffersData(
            JobOffersGenerator generator, List<CompanyIdentity> companies)
        {
            var generatedData = new List<(CompanyIdentity, IOfferData)>();
            foreach (var companyIdentity in companies)
            {
                for (int i = 0; i < offersCount; i++)
                {
                    var data = generator.GenerateData(companyIdentity);
                    generatedData.Add((companyIdentity, data));
                }
            }
            return generatedData;
        }

        private void ShuffleOffers(List<(CompanyIdentity, IOfferData)> offers)
        {
            Random rnd = new Random();
            offers = offers.OrderBy((item) => rnd.Next()).ToList();
        }

        private async Task CreateOffers(List<(CompanyIdentity, IOfferData)> offers)
        {
            foreach (var offer in offers)
            {
                var addNewOfferCommand = new AddOfferCommand(offer.Item1.ProfileId,
                                                             offer.Item2,
                                                             offersRepository);
                await addNewOfferCommand.Execute();
            }
        }
    }
}
