using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteAllCompaniesCommand : ICommand
    {
        private readonly IOffersManager offersManager;
        private readonly IRepository<CompanyIdentity> repository;
        private readonly IRepository<CompanyProfile> profileRepository;
        private readonly IUserProfileImagesStorage imagesStorage;


        public DeleteAllCompaniesCommand(
            IOffersManager offersManager,
            IRepository<CompanyIdentity> repository,
            IRepository<CompanyProfile> profileRepository,
            IUserProfileImagesStorage imagesStorage)
        {
            this.offersManager = offersManager;
            this.repository = repository;
            this.profileRepository = profileRepository;
            this.imagesStorage = imagesStorage;
        }

        public async Task Execute()
        {
            var allRecords = await repository.GetAll();
            foreach (var offer in allRecords)
            {
                await Delete(offer.Id);
            }
        }

        private async Task Delete(int id)
        {
            var deleteCommand = new DeleteCompanyCommand(
                    repository,
                    profileRepository,
                    offersManager,
                    imagesStorage,
                    id);
            await deleteCommand.Execute();
        }
    }
}
