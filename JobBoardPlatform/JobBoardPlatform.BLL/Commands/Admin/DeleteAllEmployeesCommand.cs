using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob.Temporary;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Admin
{
    /// <summary>
    /// Immediate delete
    /// </summary>
    public class DeleteAllEmployeesCommand : ICommand
    {
        private readonly IRepository<EmployeeIdentity> repository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IUserProfileImagesStorage imagesStorage;
        private readonly IProfileResumeBlobStorage resumesStorage;


        public DeleteAllEmployeesCommand(
            IRepository<EmployeeIdentity> repository,
            IRepository<EmployeeProfile> profileRepository,
            IUserProfileImagesStorage imagesStorage,
            IProfileResumeBlobStorage resumesStorage)
        {
            this.repository = repository;
            this.profileRepository = profileRepository;
            this.imagesStorage = imagesStorage;
            this.resumesStorage = resumesStorage;
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
            var deleteCommand = new DeleteEmployeeCommand(
                repository, 
                profileRepository,
                imagesStorage,
                resumesStorage,
                id);
            await deleteCommand.Execute();
        }
    }
}
