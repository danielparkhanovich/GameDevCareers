using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public class DeleteEmployeeResumeCommand : ICommand
    {
        private readonly int id;
        private readonly IRepository<EmployeeProfile> repository;
        private readonly IProfileResumeBlobStorage resumeStorage;


        public DeleteEmployeeResumeCommand(int id, 
            IRepository<EmployeeProfile> repository,
            IProfileResumeBlobStorage resumeStorage)
        {
            this.id = id;
            this.repository = repository;
            this.resumeStorage = resumeStorage;
        }

        public async Task Execute()
        {
            var profile = await repository.Get(id);

            await resumeStorage.DeleteIfNotAssignedToOffersAsync(profile.ResumeUrl!);
            profile.ResumeUrl = null;

            await repository.Update(profile);
        }
    }
}
