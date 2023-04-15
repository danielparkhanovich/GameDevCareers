using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer.CompanyBoard;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    // TODO: split into two or more controllers
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class CompanyOffersPanelController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IMapper<NewOfferViewModel, JobOffer> viewModelToOffer;


        public CompanyOffersPanelController(IRepository<JobOffer> offersRepository, 
            IRepository<TechKeyword> keywordsRepository)
        {
            this.offersRepository = offersRepository;
            this.viewModelToOffer = new NewOfferViewModelToJobOfferMapper(keywordsRepository);
        }

        public async virtual Task<IActionResult> Offers()
        {
            int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));
            var viewModelFactory = new CompanyOffersViewModelFactory(profileId, offersRepository);

            var model = await viewModelFactory.Create();

            return View(model);
        }

        public async virtual Task<IActionResult> AddOffer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> AddOffer(NewOfferViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int profileId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

                var offer = new JobOffer();
                offer.CompanyProfileId = profileId;
                offer.CreatedAt = DateTime.Now;

                viewModelToOffer.Map(viewModel, offer);

                await offersRepository.Add(offer);

                return RedirectToAction("Offers");
            }

            return View(viewModel);
        }
    }
}
