using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class PostApplicationFormCommand : ICommand
    {
        private readonly IRepository<JobOfferApplication> applicationsRepository;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage resumeStorage;
        private readonly IApplicationForm form;
        private readonly int offerId;
        private readonly int? userProfileId;


        public PostApplicationFormCommand(
            IRepository<JobOfferApplication> applicationsRepository,
            IRepository<JobOffer> offersRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage resumeStorage,
            IApplicationForm form,
            int offerId,
            int? userProfileId)
        {
            this.applicationsRepository = applicationsRepository;
            this.offersRepository = offersRepository;
            this.profileResumeStorage = profileResumeStorage;
            this.resumeStorage = resumeStorage;
            this.form = form;
            this.offerId = offerId;
            this.userProfileId = userProfileId;
        }

        public async Task Execute()
        {
            var application = await GetApplication();
            await applicationsRepository.Add(application);

            var offer = await offersRepository.Get(offerId);
            offer.NumberOfApplications += 1;
            await offersRepository.Update(offer);
        }

        private async Task<JobOfferApplication> GetApplication()
        {
            var application = new JobOfferApplication();
            MapPersonalInformation(application);
            await AddAttachedResume(application);

            return application;
        }

        private void MapPersonalInformation(JobOfferApplication application)
        {
            application.CreatedAt = DateTime.Now;
            application.ApplicationFlagTypeId = 1;
            application.JobOfferId = offerId;
            application.FullName = form.FullName;
            application.Email = form.Email;
            application.Description = form.AdditionalInformation;
            application.EmployeeProfileId = userProfileId;
        }

        private async Task AddAttachedResume(JobOfferApplication application)
        {
            if (form.AttachedResume.ResumeUrl != null)
            {
                await profileResumeStorage.AssignResumeToOfferAsync(offerId, form.AttachedResume.ResumeUrl);
                application.ResumeUrl = form.AttachedResume.ResumeUrl;
            }
            else if (form.AttachedResume.File != null)
            {
                var url = await resumeStorage.AssignResumeToOfferAsync(offerId, form.AttachedResume.File);
                application.ResumeUrl = url;
            }
        }
    }
}
