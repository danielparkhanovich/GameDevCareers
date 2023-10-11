using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.BLL.Services.Actions.Offers.Factory;
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using Microsoft.AspNetCore.Authorization;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using FluentValidation;
using JobBoardPlatform.PL.Aspects.DataValidators;
using JobBoardPlatform.PL.Controllers.Utils;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.OfferPublishedOrOwnerOnlyPolicy)]
    public class OfferContentController : Controller
    {
        private readonly IApplicationsManager applicationsManager;
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IProfileResumeBlobStorage resumeStorage;
        private readonly IOfferActionHandlerFactory actionHandlerFactory;
        private readonly IValidator<OfferApplicationUpdateViewModel> applicationFormValidator;
        private readonly ApplicationEmailViewRenderer viewRenderService;


        public OfferContentController(
            IApplicationsManager applicationsManager,
            IOfferQueryExecutor queryExecutor,
            IRepository<JobOffer> offersRepository,
            IRepository<EmployeeProfile> profileRepository,
            IRepository<EmployeeIdentity> identityRepository,
            IOfferActionHandlerFactory actionHandlerFactory,
            IProfileResumeBlobStorage resumeStorage,
            IValidator<OfferApplicationUpdateViewModel> applicationFormValidator,
            ApplicationEmailViewRenderer viewRenderService)
        {
            this.applicationsManager = applicationsManager;
            this.queryExecutor = queryExecutor;
            this.offersRepository = offersRepository;
            this.profileRepository = profileRepository;
            this.identityRepository = identityRepository;
            this.resumeStorage = resumeStorage;
            this.actionHandlerFactory = actionHandlerFactory;
            this.applicationFormValidator = applicationFormValidator;
            this.viewRenderService = viewRenderService;
        }

        [Route("offer-{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(int id, string companyname, string offertitle)
        {
            var content = new OfferContentViewModel();
            content.Display = await GetOfferContentDisplayViewModel(id);

            await TryFillApplicationForm(content);
            await TryIncreaseViewsCount(id);

            // save original id (for safety reasons need to add encryption)
            content.Update.OfferId = id;

            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("offer-{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(OfferContentViewModel content)
        {
            var result = await applicationFormValidator.ValidateAsync(content.Update);
            if (result.IsValid)
            {
                int offerId = content.Update.OfferId;
                viewRenderService.SetController(this);

                var actionsHandler = actionHandlerFactory.GetApplyActionHandler(offerId);
                if (!actionsHandler.IsActionDoneRecently(Request))
                {
                    await applicationsManager.PostApplicationFormAsync(
                        offerId, TryGetUserProfileId(), content.Update, viewRenderService);

                    actionsHandler.RegisterAction(Request, Response);
                }
            }
            else
            {
                result.AddToModelState(this.ModelState, nameof(content.Update));
            }

            content.Display = await GetOfferContentDisplayViewModel(content.Update.OfferId);
            return View(content);
        }

        private async Task<OfferContentDisplayViewModel> GetOfferContentDisplayViewModel(int offerId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);

            var viewModelFactory = new OfferContentDisplayViewModelFactory();
            return viewModelFactory.Create(offer);
        }

        private async Task TryIncreaseViewsCount(int offerId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);

            if (!IsIncreaseOfferViewsCount(offer))
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

        private async Task TryFillApplicationForm(OfferContentViewModel content)
        {
            if (!IsUserRegistered())
            {
                return;
            }

            var applicationUpdateFactory = new OfferApplicationUpdateViewModelFactory(
                User, identityRepository, profileRepository);
            var update = await applicationUpdateFactory.CreateAsync();
            await TryFillResumeField(update);

            content.Update = update;
        }

        private async Task TryFillResumeField(OfferApplicationUpdateViewModel update)
        {
            string? resumeUrl = update.AttachedResume.ResumeUrl;
            BlobDescription metadata = await resumeStorage.GetMetadataAsync(resumeUrl);
            update.AttachedResume.FileName = metadata.Name;
            update.AttachedResume.FileSize = metadata.Size;
        }

        private bool IsIncreaseOfferViewsCount(JobOffer offer)
        {
            return UserSessionUtils.IsLoggedIn(User) &&
                   !UserRolesUtils.IsUserOwner(User, offer) && 
                   !UserRolesUtils.IsUserAdmin(User);
        }

        private bool IsUserRegistered()
        {
            return UserSessionUtils.IsLoggedIn(User) && UserRolesUtils.IsUserEmployee(User);
        }

        private int? TryGetUserProfileId()
        {
            int? profileId = null;
            if (UserSessionUtils.IsLoggedIn(User))
            {
                profileId = UserSessionUtils.GetProfileId(User);
            }
            return profileId;
        }
    }
}
