using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob.Temporary;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Identities
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteEmployeeCommand : ICommand
    {
        private readonly IRepository<EmployeeIdentity> repository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IUserProfileImagesStorage imagesStorage;
        private readonly IProfileResumeBlobStorage resumesStorage;
        private readonly int idToDelete;


        public DeleteEmployeeCommand(
            IRepository<EmployeeIdentity> repository,
            IRepository<EmployeeProfile> profileRepository,
            IUserProfileImagesStorage imagesStorage,
            IProfileResumeBlobStorage resumesStorage,
            int idToDelete)
        {
            this.repository = repository;
            this.profileRepository = profileRepository;
            this.imagesStorage = imagesStorage;
            this.resumesStorage = resumesStorage;
            this.idToDelete = idToDelete;
        }

        public async Task Execute()
        {
            var user = await repository.Get(idToDelete);
            var profile = await profileRepository.Get(user.ProfileId);

            await DeleteDataFromFileStorages(profile);
            await repository.Delete(idToDelete);
            await profileRepository.Delete(profile.Id);
        }

        private async Task DeleteDataFromFileStorages(EmployeeProfile profile)
        {
            await imagesStorage.DeleteImageIfExistsAsync(profile.ProfileImageUrl);
            await resumesStorage.DetachResumeFromProfileAndTryDeleteAsync(profile.ResumeUrl);
        }
    }
}
