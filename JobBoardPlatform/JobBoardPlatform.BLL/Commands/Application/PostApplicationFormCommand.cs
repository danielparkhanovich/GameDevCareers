using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Services.IdentityVerification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class PostApplicationFormCommand : ICommand
    {
        private readonly IRepository<JobOfferApplication> applicationsRepository;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage resumeStorage;
        private readonly ApplicationForm form;
        private readonly int offerId;
        private readonly int? userProfileId;

        private readonly IApplicationsEmailSender emailSender;


        public PostApplicationFormCommand(
            IRepository<JobOfferApplication> applicationsRepository,
            IRepository<JobOffer> offersRepository,
            IProfileResumeBlobStorage profileResumeStorage,
            IApplicationsResumeBlobStorage resumeStorage,
            ApplicationForm form,
            int offerId,
            int? userProfileId,
            IApplicationsEmailSender emailSender)
        {
            this.applicationsRepository = applicationsRepository;
            this.offersRepository = offersRepository;
            this.profileResumeStorage = profileResumeStorage;
            this.resumeStorage = resumeStorage;
            this.form = form;
            this.offerId = offerId;
            this.userProfileId = userProfileId;
            this.emailSender = emailSender;
        }

        public async Task Execute()
        {
            var application = await GetApplication();
            await applicationsRepository.Add(application);

            var offer = await offersRepository.Get(offerId);
            offer.NumberOfApplications += 1;
            await offersRepository.Update(offer);

            await TrySendEmail(application, offer);
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

        private async Task TrySendEmail(JobOfferApplication application, JobOffer offer)
        {
            if (emailSender == null)
            {
                return;
            }

            if (offer.ContactDetails.ContactTypeId == ((int)ContactTypeEnum.Mail + 1))
            {
                await emailSender.SendEmailAsync(offer.ContactDetails.ContactAddress!, application);
            }
        }
    }
}
