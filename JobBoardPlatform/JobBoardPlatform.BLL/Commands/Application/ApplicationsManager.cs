using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class ApplicationsManager : IApplicationsManager
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<JobOfferApplication> applicationsRepository;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage resumeStorage;
        private readonly IEmailSender emailSender;


        public ApplicationsManager(
            IRepository<JobOffer> offersRepository, 
            IRepository<JobOfferApplication> applicationsRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage resumeStorage,
            IEmailSender emailSender)
        {
            this.offersRepository = offersRepository;
            this.applicationsRepository = applicationsRepository;
            this.profileResumeStorage = profileResumeStorage;
            this.resumeStorage = resumeStorage;
            this.emailSender = emailSender;
        }

        public async Task<ICollection<JobOfferApplication>> GetApplicationsAsync(int offerId)
        {
            var offer = (await offersRepository.GetAllSet()).Where(x => x.Id == offerId);
            var loader = new OfferWithApplicationsQueryLoader();
            var loaded = loader.Load(offer);
            return (await loaded.SingleAsync()).OfferApplications;
        }

        public async Task<JobOfferApplication> GetApplicationAsync(int applicationId)
        {
            var application = (await applicationsRepository.GetAllSet()).Where(x => x.Id == applicationId);
            var loader = new ApplicationQueryLoader();
            var loaded = loader.Load(application);
            return await loaded.SingleAsync();
        }

        public async Task PostApplicationFormAsync(
            int offerId, 
            int? userProfileId,
            ApplicationForm form,
            IEmailContent<JobOfferApplication> emailContent)
        {
            var command = new PostApplicationFormCommand(
                    applicationsRepository,
                    offersRepository,
                    profileResumeStorage,
                    resumeStorage,
                    form,
                    offerId,
                    userProfileId,
                    emailContent,
                    emailSender);
            await command.Execute();
        }

        public async Task RedirectApplicationFormAsync(int offerId)
        {
            var command = new RedirectApplicationFormCommand(
                offersRepository, offerId);
            await command.Execute();
        }

        /// <returns>result priority value</returns>
        public async Task<int> UpdateApplicationPriorityAsync(int applicationId, int newPriorityIndex)
        {
            var updateApplicationPriorityCommand = new UpdateApplicationPriorityCommand(
                applicationsRepository, applicationId, newPriorityIndex);
            await updateApplicationPriorityCommand.Execute();

            return updateApplicationPriorityCommand.Result;
        }
    }
}
