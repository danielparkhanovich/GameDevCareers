using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class UpdateApplicationPriorityCommand : ICommand
    {
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly int applicationId;
        private readonly int newPriorityIndex;


        public UpdateApplicationPriorityCommand(IRepository<OfferApplication> applicationsRepository, 
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
        }
    }
}
