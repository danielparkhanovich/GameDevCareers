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
            this.actionHandlerFactory = actionHandlerFactory;
        }

        [Route("offer-{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(int id, string companyname, string offertitle)
        {
            var offer = await offersRepository.Get(id);

            var viewModelFactory = new OfferContentDisplayViewModelFactory(offer.Id, offersRepository);
            var display = await viewModelFactory.Create();

            var content = new OfferContentViewModel();
            content.Display = display;

            if (!offer.IsPublished)
            {
                if (UserRolesUtils.IsUserOwner(User, offer))
                {
                    // Pass only owners
                    return View(content);
                }
                return RedirectToAction("Index", "Home");
            }

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

            if (content.File == null && string.IsNullOrEmpty(content.ResumeUrl))
            {
                return RedirectToAction("Offer");
            }
            if (content.File == null && string.IsNullOrEmpty(await resumeStorage.GetBlobName(content.ResumeUrl)))
            {
                return RedirectToAction("Offer");
            }

            int offerId = content.Update.OfferId;

            var actionsHandler = actionHandlerFactory.GetApplyActionHandler(offerId);
            if (!actionsHandler.IsActionDoneRecently(Request))
            {
                var postFormCommand = new PostApplicationFormCommand(applicationsRepository,
                    offersRepository,
                    azureOptions,
                    User,
                    offerId,
                    content.Update);
                await postFormCommand.Execute();

                actionsHandler.RegisterAction(Request, Response);
            }

            return RedirectToAction("Offer");
        }

        private async Task TryIncreaseViewsCount(JobOffer offer)
        {
            if (UserRolesUtils.IsUserOwner(User, offer))
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
            bool isUserLoggedIn = User.Identity.IsAuthenticated;

            if (!isUserLoggedIn)
            {
                return;
            }

            string userRole = UserSessionUtils.GetRole(User);

            if (userRole != UserRoles.Employee)
            {
                return;
            }

            var applicationUpdateFactory = new OfferApplicationUpdateViewModelFactory(User,
                identityRepository,
                profileRepository);
            var update = await applicationUpdateFactory.Create();

            string? resumeUrl = update.AttachedResume.ResumeUrl;

            if (resumeUrl != null)
            {
                update.AttachedResume.FileName = await resumeStorage.GetBlobName(resumeUrl);
                update.AttachedResume.FileSize = await resumeStorage.GetBlobSize(resumeUrl);
            }

            content.Update = update;
        }
    }
}
