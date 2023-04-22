using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using System.Security.Claims;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Users;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob;
using Microsoft.Extensions.Options;
using JobBoardPlatform.PL.ViewModels.Profile.Employee;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer.Application;
using JobBoardPlatform.BLL.Services.Actions.Offers.Factory;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    public class OfferContentController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IRepository<OfferApplication> applicationsRepository;
        private readonly IBlobStorage resumeStorage;
        private readonly IOptions<AzureOptions> azureOptions;
        private readonly IMapper<OfferApplicationUpdateViewModel, OfferApplication> applicationViewToModel;
        private readonly IOfferActionHandlerFactory actionHandlerFactory;


        public OfferContentController(IRepository<JobOffer> offersRepository,
            IRepository<EmployeeProfile> profileRepository,
            IRepository<EmployeeIdentity> identityRepository,
            IRepository<OfferApplication> applicationsRepository,
            IOptions<AzureOptions> azureOptions,
            IOfferActionHandlerFactory actionHandlerFactory)
        {
            this.offersRepository = offersRepository;
            this.profileRepository = profileRepository;
            this.identityRepository = identityRepository;
            this.applicationsRepository = applicationsRepository;

            this.resumeStorage = new UserProfileAttachedResumeStorage(azureOptions);
            this.azureOptions = azureOptions;
            this.applicationViewToModel = new OfferApplicationViewModelToModelMapper();
            this.actionHandlerFactory = actionHandlerFactory;
        }

        [Route("{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(int id, string companyname, string offertitle)
        {
            var offer = await offersRepository.Get(id);

            var viewModelFactory = new OfferContentDisplayViewModelFactory(offer.Id, offersRepository);
            var display = await viewModelFactory.Create();

            var content = new OfferContentViewModel();
            content.Display = display;

            if (!offer.IsPublished)
            {
                if (IsUserOwner(offer))
                {
                    // Pass only owners
                    return View(content);
                }
                return RedirectToAction("Index", "Home");
            }

            await TryFillApplicationForm(content, display);

            await TryIncreaseViewsCount(offer);

            // save original id (for safety reasons need to add encryption)
            content.Update.OfferId = id;

            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(OfferContentViewModel content)
        {
            if (ModelState.IsValid)
            {
                if (content.File == null && string.IsNullOrEmpty(content.ResumeUrl))
                {
                    return RedirectToAction("Offer");
                }
                if (content.File == null && string.IsNullOrEmpty(await resumeStorage.GetBlobName(content.ResumeUrl)))
                {
                    return RedirectToAction("Offer");
                }
                int offerId = content.Update.OfferId;

                var application = new OfferApplication();

                var actionsHandler = actionHandlerFactory.GetApplyActionHandler(offerId);
                if (!actionsHandler.IsActionDoneRecently(Request))
                {
                    await PostApplicationForm(offerId, content, application);

                    var offer = await offersRepository.Get(offerId);
                    offer.NumberOfApplications += 1;

                    await offersRepository.Update(offer);

                    actionsHandler.RegisterAction(Request, Response);
                }
            }

            return RedirectToAction("Offer");
        }

        private async Task TryIncreaseViewsCount(JobOffer offer)
        {
            if (IsUserOwner(offer))
            {
                return;
            }

            var actionsHandler = actionHandlerFactory.GetViewActionHandler(offer.Id);
            if (actionsHandler.IsActionDoneRecently(Request))
            {
                return;
            }

            offer.NumberOfViews += 1;

            await offersRepository.Update(offer);

            actionsHandler.RegisterAction(Request, Response);
        }

        private async Task TryFillApplicationForm(OfferContentViewModel content, OfferContentDisplayViewModel display)
        {
            bool isUserLoggedIn = User.Identity.IsAuthenticated;

            if (isUserLoggedIn)
            {
                string userRole = User.FindFirstValue(UserSessionProperties.Role);
                
                if (userRole == UserRoles.Employee)
                {
                    var update = await GetFilledUpdateViewModel();
                    content.Update = update;

                    string? resumeUrl = update.AttachedResume.ResumeUrl;

                    if (resumeUrl != null)
                    {
                        update.AttachedResume.FileName = await resumeStorage.GetBlobName(resumeUrl);
                        update.AttachedResume.FileSize = await resumeStorage.GetBlobSize(resumeUrl);
                    }
                }
            }
        }

        private async Task<OfferApplicationUpdateViewModel> GetFilledUpdateViewModel()
        {
            var update = new OfferApplicationUpdateViewModel();

            // Auto fill form
            int identityId = UserSessionProperties.GetIdentityId(User);

            var identity = await identityRepository.Get(identityId);
            var profile = await profileRepository.Get(identity.ProfileId);

            update.FullName = $"{profile.Name} {profile.Surname}";
            update.Email = identity.Email;

            var attachedResume = new EmployeeAttachedResumeViewModel();
            attachedResume.ResumeUrl = profile.ResumeUrl;
            update.AttachedResume = attachedResume;

            if (profile.Description != null)
            {
                update.AdditionalInformation = profile.Description;
            }

            return update;
        }

        private bool IsUserOwner(JobOffer offer)
        {
            bool isUserLoggedIn = User.Identity.IsAuthenticated;

            if (isUserLoggedIn)
            {
                string userRole = User.FindFirstValue(UserSessionProperties.Role);
                int userId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
                bool isOwner = ((userRole == UserRoles.Company) && (offer.CompanyProfileId == userId));

                return isOwner;
            }

            return false;
        }

        // TODO: refactor -> create all models using specific factory, if model is complex
        // TODO: also add validator.
        // TODO: 1. validate the view model, return an error in case of problems
        // TODO: 2. create using factory the actual model
        private async Task PostApplicationForm(int offerId, OfferContentViewModel content, OfferApplication application)
        {
            application.CreatedAt = DateTime.Now;
            application.JobOfferId = offerId;

            bool isUserLoggedIn = User.Identity.IsAuthenticated;
            if (isUserLoggedIn)
            {
                int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
                application.EmployeeProfileId = profileId;
            }

            if (content.Update.AttachedResume.ResumeUrl != null) 
            {
                application.ResumeUrl = content.Update.AttachedResume.ResumeUrl;
            }
            else if (content.Update.AttachedResume.File != null) 
            {
                var applicationsResumesStorage = new UserApplicationsResumeStorage(azureOptions, offerId);
                var url = await applicationsResumesStorage.UpdateAsync(null, content.Update.AttachedResume.File);
                application.ResumeUrl = url;
            }

            applicationViewToModel.Map(content.Update, application);

            await applicationsRepository.Add(application);
        }
    }
}
