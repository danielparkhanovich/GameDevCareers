using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.PL.ViewModels.JobOffer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.JobOffers
{
    [Authorize(Policy = AuthorizationPolicies.CompanyOnlyPolicy)]
    public class JobOffersController : Controller
    {
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
        public async virtual Task<IActionResult> AddOffer(JobOfferUpdateViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
