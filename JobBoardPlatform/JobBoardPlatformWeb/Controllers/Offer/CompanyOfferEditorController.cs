using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOfferEditorController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<TechKeyword> keywordsRepository;


        public CompanyOfferEditorController(IRepository<JobOffer> offersRepository,
            IRepository<TechKeyword> keywordsRepository)
        {
            this.offersRepository = offersRepository;
            this.keywordsRepository = keywordsRepository;
        }

        public IActionResult NewOffer()
        {
            return View();
        }

        [Route("edit-offer-{offerId}")]
        public async Task<IActionResult> NewOffer(int offerId)
        {
            var viewModelFactory = new OfferDetailsViewModelFactory(offerId,
                offersRepository);

            var viewModel = await viewModelFactory.Create();

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-offer-{offerId}")]
        public async Task<IActionResult> EditOffer(int offerId, OfferDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int profileId = UserSessionUtils.GetProfileId(User);

                var addNewOfferCommand = new AddNewOfferCommand(profileId,
                    viewModel,
                    keywordsRepository,
                    offersRepository);

                await addNewOfferCommand.Execute();

                return RedirectToAction("Offers", "CompanyOffersPanel");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOffer(OfferDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int profileId = UserSessionUtils.GetProfileId(User);

                var addNewOfferCommand = new AddNewOfferCommand(profileId,
                    viewModel,
                    keywordsRepository,
                    offersRepository);

                await addNewOfferCommand.Execute();

                return RedirectToAction("Offers", "CompanyOffersPanel");
            }

            return View(viewModel);
        }
    }
}
