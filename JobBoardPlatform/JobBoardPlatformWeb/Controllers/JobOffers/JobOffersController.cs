using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Company.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.JobOffers
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class JobOffersController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IMapper<JobOfferCardDisplayViewModel, JobOffer> viewModelToOffer;


        public JobOffersController(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
            this.viewModelToOffer = new JobOfferViewModelToJobOfferMapper();
        }

        public async virtual Task<IActionResult> Offers()
        {
            return View();
        }

        public async virtual Task<IActionResult> AddOffer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> AddOffer(JobOfferCardDisplayViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

                var offer = new JobOffer();
                offer.CompanyProfileId = id;
                offer.CreatedAt = DateTime.Now;

                viewModelToOffer.Map(viewModel, offer);

                await offersRepository.Add(offer);

                return RedirectToAction("Offers");
            }

            return View(viewModel);
        }
    }
}
