using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
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
    public class AdminPanelEmployeesController : CardsControllerBase
    {
        public const string LogIntoAction = "LogInto";
        public const string DeleteAction = "Delete";

        private readonly IRepository<EmployeeIdentity> identityRepository;


        public AdminPanelEmployeesController(IRepository<EmployeeIdentity> identityRepository)
        {
            this.identityRepository = identityRepository;
        }

        public async Task<IActionResult> Panel()
        {
            var viewModel = new AdminPanelEmployeesViewModel();
            viewModel.CardsContainer = await GetContainer();
            viewModel.AllRecords = await identityRepository.GetAll();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogInto(int userId)
        {
            // var deleteCommand = new DeleteCompanyCommand(companyIdentityRepository, userId);
            // await deleteCommand.Execute();

            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            var deleteCommand = new DeleteEmployeeCommand(identityRepository, userId);
            await deleteCommand.Execute();

            return RedirectToAction("Panel");
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var searchParameters = GetSearchParametersFromUrl();
            var containerFactory = new AdminPanelEmployeesContainerViewModelFactory(identityRepository,
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
