using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers.Offer
{
    public class OfferContentController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;


        public OfferContentController(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        [Route("{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(int id, string companyname, string offertitle)
        {
            var offer = await offersRepository.Get(id);

            var viewModelFactory = new OfferContentViewModelFactory(id, offersRepository);
            var content = await viewModelFactory.Create();

            if (!offer.IsPublished)
            {
                if (User.Identity.IsAuthenticated)
                {
                    int userId = int.Parse(User.FindFirstValue(UserSessionProperties.ProfileIdentifier));

                    // Pass only creators
                    if (offer.CompanyProfileId == userId)
                    {
                        return View(content);
                    }
                }
                return RedirectToAction("Index", "Home");
            }

            return View(content);
        }
    }
}
