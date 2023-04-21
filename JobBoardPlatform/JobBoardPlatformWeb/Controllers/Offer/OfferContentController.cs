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
using JobBoardPlatform.BLL.Services.PageViews;
using JobBoardPlatform.PL.ViewModels.Profile.Employee;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    public class OfferContentController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IBlobStorage resumeStorage;


        public OfferContentController(IRepository<JobOffer> offersRepository,
            IRepository<EmployeeProfile> profileRepository,
            IRepository<EmployeeIdentity> identityRepository,
            IOptions<AzureOptions> azureOptions)
        {
            this.offersRepository = offersRepository;
            this.profileRepository = profileRepository;
            this.identityRepository = identityRepository;

            this.resumeStorage = new UserProfileAttachedResumeStorage(azureOptions);
        }

        [Route("{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(int id, string companyname, string offertitle)
        {
            var offer = await offersRepository.Get(id);

            var viewModelFactory = new OfferContentDisplayViewModelFactory(offer.Id, offersRepository);
            var display = await viewModelFactory.Create();

            var content = new OfferContentViewModel();
            content.Display = display;
            content.Update = new OfferApplicationUpdateViewModel()
            {
                AttachedResume = new EmployeeAttachedResumeViewModel()
            };

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

            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Offer(OfferContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // TODO: custom validation check for CV url and file

                int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

                var offer = new JobOffer();
                offer.CompanyProfileId = profileId;
                offer.CreatedAt = DateTime.Now;

                // viewModelToOffer.Map(viewModel, offer);

                await offersRepository.Add(offer);

                return RedirectToAction("Offers");
            }

            return View(viewModel);
        }

        private async Task TryIncreaseViewsCount(JobOffer offer)
        {
            if (IsUserOwner(offer))
            {
                return;
            }

            var viewsHandler = new UserViewsHandler();
            if (viewsHandler.IsViewedRecently(offer.Id, Request, Response))
            {
                return;
            }

            offer.NumberOfViews += 1;

            await offersRepository.Update(offer);
        }

        private async Task TryFillApplicationForm(OfferContentViewModel content, OfferContentDisplayViewModel display)
        {
            bool isUserLoggedIn = User.Identity.IsAuthenticated;

            if (isUserLoggedIn)
            {
                string userRole = User.FindFirstValue(UserSessionProperties.Role);
                
                if (userRole == UserRoles.Employee)
                {
                    var update = await GetUpdateViewModel();
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

        private async Task<OfferApplicationUpdateViewModel> GetUpdateViewModel()
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
    }
}
