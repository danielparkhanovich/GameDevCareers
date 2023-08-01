using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteCompanyCommand : ICommand
    {
        private readonly IRepository<CompanyIdentity> repository;
        private readonly IRepository<CompanyProfile> profileRepository;
        private readonly IOfferManager offersManager;
        private readonly IUserProfileImagesStorage imagesStorage;
        private readonly int idToDelete;


        public DeleteCompanyCommand(
            IRepository<CompanyIdentity> repository,
            IRepository<CompanyProfile> profileRepository,
            IOfferManager offersManager,
            IUserProfileImagesStorage imagesStorage,
            int idToDelete)
        {
            this.repository = repository;
            this.profileRepository = profileRepository;
            this.offersManager = offersManager;
            this.imagesStorage = imagesStorage;
            this.idToDelete = idToDelete;
        }

        public async Task Execute()
        {
            var user = await repository.Get(idToDelete);
            var profile = await profileRepository.Get(user.ProfileId);

            await DeleteCompanyOffers(profile);
            await DeleteCompanyProfile(profile);
        }

        private async Task DeleteCompanyProfile(CompanyProfile profile)
        {
            await DeleteDataFromFileStorages(profile);
            await profileRepository.Delete(profile.Id);
            await repository.Delete(idToDelete);
        }

        private async Task DeleteCompanyOffers(CompanyProfile profile)
        {
            var offersIds = await offersManager.GetAllIdsAsync(profile.Id);
            foreach (var id in offersIds)
            {
                await offersManager.DeleteAsync(id);
            }
        }

        private async Task DeleteDataFromFileStorages(CompanyProfile profile)
        {
            await imagesStorage.DeleteImageIfExistsAsync(profile.ProfileImageUrl);
        }
    }
}
