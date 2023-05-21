using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize]
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminController : Controller
    {
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
            var container = await GetCardsContainer();

            var viewModel = new AdminPanelViewModel();
            viewModel.OffersContainer = container;
            viewModel.AllCompanies = await companyRepository.GetAll();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOffers(int companyId, int offersCountToGenerate)
        {
            bool isOverLimit = await IsGenerateCountOverLimit(offersCountToGenerate);
            if (isOverLimit)
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
        public async virtual Task<IActionResult> RefreshCardContainer()
        {
            var searchParameters = GetSearchParametersFromUrl();
            var containerFactory = new AdminPanelOffersContainerViewModelFactory(offerRepository, searchParameters!);

            var model = await containerFactory.Create();

            return PartialView("./Templates/_CardsContainer", model);
        }

        private async Task<bool> IsGenerateCountOverLimit(int countToGenerate)
        {
            int generateOffersLimit = 500;

            int totalToGenerate = countToGenerate;
            if (countToGenerate == -1)
            {
                var companiesCount = await companyRepository.GetAllSet();
                totalToGenerate *= companiesCount.Count();
            }

            if (totalToGenerate > generateOffersLimit)
            {
                return true;
            }

            return false;
        }

        private async Task<CardsContainerViewModel> GetCardsContainer()
        {
            var searchParams = GetSearchParametersFromUrl();
            var containerFactory = new AdminPanelOffersContainerViewModelFactory(offerRepository, searchParams);
            var container = await containerFactory.Create();

            return container;
        }

        private CompanyPanelOfferSearchParameters GetSearchParametersFromUrl()
        {
            int? profileId = null;
            var offerSearchParametersFactory = new CompanyPanelOfferSearchParametersFactory(Request, profileId);
            var searchParams = offerSearchParametersFactory.Create();

            return searchParams;
        }
    }
}
