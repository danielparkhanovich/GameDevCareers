using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public class DeleteEmployeeResumeCommand : ICommand
    {
        private readonly int id;
        private readonly IRepository<EmployeeProfile> repository;
        private readonly IProfileResumeBlobStorage resumeStorage;
        private readonly IUserSessionService<EmployeeIdentity, EmployeeProfile> userSession;
        private readonly HttpContext httpContext;


        public DeleteEmployeeResumeCommand(int id, 
            IRepository<EmployeeProfile> repository,
            IProfileResumeBlobStorage resumeStorage,
            IUserSessionService<EmployeeIdentity, EmployeeProfile> userSession,
            HttpContext httpContext)
        {
            this.id = id;
            this.repository = repository;
            this.resumeStorage = resumeStorage;
            this.userSession = userSession;
            this.httpContext = httpContext;
        }

        public async Task Execute()
        {
            var profile = await repository.Get(id);

            await resumeStorage.DeleteIfNotAssignedToOffersAsync(profile.ResumeUrl!);
            profile.ResumeUrl = null;

            await repository.Update(profile);
            await userSession.UpdateSessionStateAsync(httpContext);
        }
    }
}
