using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class UpdateApplicationPriorityCommand : ICommandWithResult<int>
    {
        private readonly IRepository<JobOfferApplication> applicationsRepository;
        private readonly int applicationId;
        private readonly int newPriorityIndex;


        public int Result { get; private set; }


        public UpdateApplicationPriorityCommand(IRepository<JobOfferApplication> applicationsRepository, 
            int applicationId, 
            int newPriorityIndex)
        {
            this.applicationsRepository = applicationsRepository;
            this.applicationId = applicationId;
            this.newPriorityIndex = newPriorityIndex;
        }

        public async Task Execute()
        {
            var application = await applicationsRepository.Get(applicationId);

            // double select -> deselect
            if (application.ApplicationFlagTypeId == newPriorityIndex)
            {
                application.ApplicationFlagTypeId = 1;
            }
            else
            {
                application.ApplicationFlagTypeId = newPriorityIndex;
            }

            await applicationsRepository.Update(application);

            Result = application.ApplicationFlagTypeId;
        }
    }
}
