using JobBoardPlatform.BLL.Commands.Contracts;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace JobBoardPlatform.BLL.Commands.Application
{
    public class PostApplicationFormCommand : ICommand
    {
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IOptions<AzureOptions> azureOptions;
        private readonly int offerId;
        private readonly ClaimsPrincipal user;
        private readonly IApplicationForm form;


        public PostApplicationFormCommand(IRepository<OfferApplication> applicationsRepository,
            IRepository<JobOffer> offersRepository,
            IOptions<AzureOptions> azureOptions,
            ClaimsPrincipal user,
            int offerId,
            IApplicationForm form)
        {
            this.applicationsRepository = applicationsRepository;
            this.offersRepository = offersRepository;
            this.azureOptions = azureOptions;
            this.user = user;
            this.offerId = offerId;
            this.form = form;
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

            application.CreatedAt = DateTime.Now;
            application.ApplicationFlagTypeId = 1;
            application.JobOfferId = offerId;

            bool isUserLoggedIn = user.Identity.IsAuthenticated;
            if (isUserLoggedIn)
            {
                int profileId = UserSessionUtils.GetIdentityId(user);
                application.EmployeeProfileId = profileId;
            }

            if (form.AttachedResume.ResumeUrl != null)
            {
                application.ResumeUrl = form.AttachedResume.ResumeUrl;
            }
            else if (form.AttachedResume.File != null)
            {
                var applicationsResumesStorage = new UserApplicationsResumeStorage(azureOptions, offerId);
                var url = await applicationsResumesStorage.UpdateAsync(null, form.AttachedResume.File);
                application.ResumeUrl = url;
            }

            application.FullName = form.FullName;
            application.Email = form.Email;
            application.Description = form.AdditionalInformation;

            return application;
        }
    }
}
