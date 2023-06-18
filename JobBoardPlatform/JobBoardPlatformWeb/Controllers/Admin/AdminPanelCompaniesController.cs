using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Login;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Route("admin-panel-companies")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminPanelCompaniesController : AdminPanelUsersControllerBase<CompanyIdentity, AdminPanelCompaniesViewModel>
    {
        private readonly IRepository<CompanyIdentity> identityRepository;
        private readonly AuthorizationService<CompanyIdentity, CompanyProfile> authorizationService;


        public AdminPanelCompaniesController(
            IRepository<CompanyIdentity> identityRepository, 
            AuthorizationService<CompanyIdentity, CompanyProfile> authorizationService) 
            : base(identityRepository)
        {
            this.identityRepository = identityRepository;
            this.authorizationService = authorizationService;
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var searchParams = GetSearchParametersFromUrl();
            var containerFactory = new AdminPanelCompaniesContainerViewModelFactory(
                identityRepository, searchParams);
            return containerFactory.CreateAsync();
        }

        private CompanyPanelOfferSearchParameters GetSearchParametersFromUrl()
        {
            int? profileId = null;
            var offerSearchParametersFactory = new CompanyPanelOfferSearchParametersFactory();
            var searchParams = offerSearchParametersFactory.GetSearchParams(Request);
            return searchParams;
        }

        protected override ICommand GetLogIntoCommand(int userId)
        {
            return new LogIntoAccountCommand<CompanyIdentity, CompanyProfile>(
                HttpContext, authorizationService, userId);
        }

        protected override ICommand GetDeleteCommand(int userId)
        {
            return new DeleteCompanyCommand(identityRepository, userId);
        }
    }
}
