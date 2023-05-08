using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Search.Offers;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminController : Controller
    {
        private const int GenerateOffersLimit = 500;

        private readonly IRepository<JobOffer> offerRepository;

        private readonly IRepository<CompanyProfile> companyRepository;
        private readonly IRepository<EmployeeProfile> employeeRepository;
        private readonly IRepository<CompanyIdentity> companyIdentityRepository;
        private readonly IRepository<CompanyIdentity> employeeIdentityRepository;
        private readonly IRepository<TechKeyword> keywordsRepository;

        public AdminController(IRepository<JobOffer> offerRepository,
            IRepository<CompanyProfile> companyRepository,
            IRepository<EmployeeProfile> employeeRepository,
            IRepository<CompanyIdentity> companyIdentityRepository,
            IRepository<CompanyIdentity> employeeIdentityRepository,
            IRepository<TechKeyword> keywordsRepository)
        {
            this.offerRepository = offerRepository;
            this.companyRepository = companyRepository;
            this.employeeRepository = employeeRepository;
            this.companyIdentityRepository = companyIdentityRepository;
            this.employeeIdentityRepository = employeeIdentityRepository;
            this.keywordsRepository = keywordsRepository;
        }

        public async Task<IActionResult> OffersPanel()
        {
            var offerSearchData = new OfferSearchData()
            {
                IsRemoteOnly = false,
                IsSalaryOnly = false,
                MainTechnology = 0,
                Page = 1,
                SearchString = string.Empty,
                Type = OfferType.Employment
            };

            var containerFactory = new AdminPanelCardsViewModelFactory(offerRepository,
                offerSearchData, 20, new bool[] { false, false }, 1);

            var container = await containerFactory.Create();

            var viewModel = new AdminPanelViewModel();
            viewModel.OffersContainer = container;
            viewModel.AllCompanies = await companyRepository.GetAll();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOffers(int companyId, int offersCountToGenerate)
        {
            int totalToGenerate = offersCountToGenerate;
            if (offersCountToGenerate == -1)
            {
                var companiesCount = await companyRepository.GetAllSet();
                totalToGenerate *= companiesCount.Count();
            }

            if (totalToGenerate > GenerateOffersLimit)
            {
                return RedirectToAction("OffersPanel");
            }

            var generateOffersCommand = new GenerateOffersCommand(offersCountToGenerate,
                companyId, 
                companyIdentityRepository, 
                keywordsRepository, 
                offerRepository);
            await generateOffersCommand.Execute();

            return RedirectToAction("OffersPanel");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllOffers()
        {
            var deleteOffersCommand = new DeleteAllOffersCommand(offerRepository);
            await deleteOffersCommand.Execute();

            return RedirectToAction("OffersPanel");
        }

        [HttpPost]
        public async virtual Task<IActionResult> RefreshCardContainer(ContainerCardsViewModel cardsViewModel)
        {
            var offerSearchData = new OfferSearchData()
            {
                IsRemoteOnly = cardsViewModel.FilterToggles[0],
                IsSalaryOnly = cardsViewModel.FilterToggles[1],
                MainTechnology = 0,
                Page = cardsViewModel.Page,
                SearchString = string.Empty,
                Type = OfferType.Employment
            };

            var viewModelFactory = new AdminPanelCardsViewModelFactory(offerRepository,
                offerSearchData, 20, cardsViewModel.FilterToggles, cardsViewModel.Page);

            var model = await viewModelFactory.Create();

            return PartialView("./Templates/_CardsContainer", model);
        }
    }
}
