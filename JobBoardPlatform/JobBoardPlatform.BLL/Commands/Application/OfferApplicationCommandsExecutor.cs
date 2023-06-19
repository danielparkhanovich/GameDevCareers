using JobBoardPlatform.BLL.Services.Actions.Offers.Factory;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using JobBoardPlatform.DAL.Options;
using System.Security.Claims;
using JobBoardPlatform.BLL.Commands.Contracts;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class OfferApplicationCommandsExecutor
    {
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly UserApplicationsResumeStorage resumeStorage;
        private readonly IOfferActionHandlerFactory actionHandlerFactory;


        public OfferApplicationCommandsExecutor(
            IRepository<OfferApplication> applicationsRepository,
            IRepository<JobOffer> offersRepository,
            UserApplicationsResumeStorage resumeStorage,
            IOfferActionHandlerFactory actionHandlerFactory)
        {
            this.applicationsRepository = applicationsRepository;
            this.offersRepository = offersRepository;
            this.resumeStorage = resumeStorage;
            this.actionHandlerFactory = actionHandlerFactory;
        }

        public async Task TryPostApplicationFormAsync(
            int offerId, ClaimsPrincipal user, HttpRequest request, HttpResponse response, IApplicationForm form)
        {
            var actionsHandler = actionHandlerFactory.GetApplyActionHandler(offerId);
            if (!actionsHandler.IsActionDoneRecently(request))
            {
                var postFormCommand = new PostApplicationFormCommand(
                    applicationsRepository, offersRepository, resumeStorage, user, offerId, form);
                await postFormCommand.Execute();

                actionsHandler.RegisterAction(request, response);
            }
        }

        /// <returns>result priority value</returns>
        public async Task<int> UpdateApplicationPriorityCommandAsync(int applicationId, int newPriorityIndex)
        {
            var updateApplicationPriorityCommand = new UpdateApplicationPriorityCommand(
                applicationsRepository, applicationId, newPriorityIndex);
            await updateApplicationPriorityCommand.Execute();

            return updateApplicationPriorityCommand.ResultPriorityIndex;
        }
    }
}
