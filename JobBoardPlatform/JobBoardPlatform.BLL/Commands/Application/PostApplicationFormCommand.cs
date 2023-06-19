using JobBoardPlatform.BLL.Commands.Contracts;
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
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly UserApplicationsResumeStorage resumeStorage;
        private readonly ClaimsPrincipal user;
        private readonly int offerId;
        private readonly IApplicationForm form;


        public PostApplicationFormCommand(
            IRepository<OfferApplication> applicationsRepository,
            IRepository<JobOffer> offersRepository,
            UserApplicationsResumeStorage resumeStorage,
            ClaimsPrincipal user,
            int offerId,
            IApplicationForm form)
        {
            this.applicationsRepository = applicationsRepository;
            this.offersRepository = offersRepository;
            this.user = user;
            this.form = form;
            this.offerId = offerId;
            this.resumeStorage = resumeStorage;
            this.resumeStorage.SetOfferIdProperty(offerId.ToString());
        }

        public async Task Execute()
        {
            var application = await GetApplication();
            await applicationsRepository.Add(application);

            var offer = await offersRepository.Get(offerId);
            offer.NumberOfApplications += 1;
            await offersRepository.Update(offer);
        }

        private async Task<OfferApplication> GetApplication()
        {
            var application = new OfferApplication();
            MapPersonalInformation(application);
            AddUserProfileIfUserLoggedIn(application);
            await AddAttachedResume(application);

            return application;
        }

        private void MapPersonalInformation(OfferApplication application)
        {
            application.CreatedAt = DateTime.Now;
            application.ApplicationFlagTypeId = 1;
            application.JobOfferId = offerId;
            application.FullName = form.FullName;
            application.Email = form.Email;
            application.Description = form.AdditionalInformation;
        }

        private void AddUserProfileIfUserLoggedIn(OfferApplication application)
        {
            if (UserSessionUtils.IsLoggedIn(user))
            {
                int profileId = UserSessionUtils.GetIdentityId(user);
                application.EmployeeProfileId = profileId;
            }
        }

        private async Task AddAttachedResume(OfferApplication application)
        {
            if (form.AttachedResume.ResumeUrl != null)
            {
                application.ResumeUrl = form.AttachedResume.ResumeUrl;
            }
            else if (form.AttachedResume.File != null)
            {
                var url = await resumeStorage.UpdateAsync(null, form.AttachedResume.File);
                application.ResumeUrl = url;
            }
        }
    }
}
