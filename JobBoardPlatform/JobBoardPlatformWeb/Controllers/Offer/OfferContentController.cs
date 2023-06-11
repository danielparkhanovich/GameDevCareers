using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Options;
using JobBoardPlatform.DAL.Repositories.Blob;
using Microsoft.Extensions.Options;
using JobBoardPlatform.BLL.Services.Actions.Offers.Factory;
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using Microsoft.AspNetCore.Authorization;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.OfferPublishedOrOwnerOnlyPolicy)]
    public class OfferContentController : Controller
    {
        private readonly ApplicationCommandsExecutor commandsExecutor;
        private readonly OfferQueryExecutor queryExecutor;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IBlobStorage resumeStorage;
        private readonly IOfferActionHandlerFactory actionHandlerFactory;


        public OfferContentController(
            ApplicationCommandsExecutor commandsExecutor,
            OfferQueryExecutor queryExecutor,
            IRepository<JobOffer> offersRepository,
            IRepository<EmployeeProfile> profileRepository,
            IRepository<EmployeeIdentity> identityRepository,
            IOfferActionHandlerFactory actionHandlerFactory,
            IOptions<AzureOptions> azureOptions)
        {
            this.commandsExecutor = commandsExecutor;
            this.queryExecutor = queryExecutor;
            this.offersRepository = offersRepository;
            this.profileRepository = profileRepository;
            this.identityRepository = identityRepository;
            this.resumeStorage = new UserProfileAttachedResumeStorage(azureOptions);
            this.actionHandlerFactory = actionHandlerFactory;
        }

        [Route("offer-{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(int id, string companyname, string offertitle)
        {
            var offer = await queryExecutor.GetOfferById(id);
            var display = GetOfferContentDisplayViewModel(offer);

            var content = new OfferContentViewModel();
            content.Display = display;

            await TryFillApplicationForm(content);
            await TryIncreaseViewsCount(offer);

            // save original id (for safety reasons need to add encryption)
            content.Update.OfferId = id;

            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("offer-{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(OfferContentViewModel content)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Offer");
            }
            else if (content.File == null && await resumeStorage.IsExistsAsync(content.ResumeUrl))
            {
                return RedirectToAction("Offer");
            }

            int offerId = content.Update.OfferId;
            await commandsExecutor.TryPostApplicationFormAsync(offerId, User, Request, Response, content.Update);

            return RedirectToAction("Offer");
        }

        private OfferContentDisplayViewModel GetOfferContentDisplayViewModel(JobOffer offer)
        {
            var viewModelFactory = new OfferContentDisplayViewModelFactory();
            return viewModelFactory.Create(offer);
        }

        private async Task TryIncreaseViewsCount(JobOffer offer)
        {
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
            FileMetadata metadata = await resumeStorage.GetBlobMetadataAsync(resumeUrl);
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
    }
}
