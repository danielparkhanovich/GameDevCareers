using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.ViewModels.Factories.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminPanelCompaniesController : CardsControllerBase
    {
        public const string DeleteAction = "DeleteCompany";
        public const string GenerateAction = "Generate";
        public const string DeleteAllCompanies = "DeleteAll";

        private readonly IRepository<CompanyIdentity> companyIdentityRepository;


        public AdminPanelCompaniesController(IRepository<CompanyIdentity> companyIdentityRepository)
        {
            this.companyIdentityRepository = companyIdentityRepository;
        }

        public async Task<IActionResult> Panel()
        {
            var viewModel = new AdminPanelCompaniesViewModel();
            viewModel.OffersContainer = await GetContainer();
            viewModel.AllRecords = await companyIdentityRepository.GetAll();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Generate(int countToGenerate)
        {
            var generateOffersCommand = new GenerateCompaniesCommand(countToGenerate, companyIdentityRepository);
            await generateOffersCommand.Execute();

            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            var deleteOffersCommand = new DeleteAllCompaniesCommand(companyIdentityRepository);
            await deleteOffersCommand.Execute();

            return RedirectToAction("Panel");
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var searchParameters = GetSearchParametersFromUrl();
            var containerFactory = new AdminPanelCompaniesContainerViewModelFactory(companyIdentityRepository, 
                searchParameters!);
            return containerFactory.Create();
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
