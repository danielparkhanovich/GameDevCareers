using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
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
        private readonly IDeleteUserCommandFactory deleteUserCommandFactory;
        private readonly IRepository<CompanyIdentity> identityRepository;
        private readonly IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService;


        public AdminPanelCompaniesController(
            IDeleteUserCommandFactory deleteUserCommandFactory,
            IRepository<CompanyIdentity> identityRepository,
            IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService) 
            : base(identityRepository)
        {
            this.deleteUserCommandFactory = deleteUserCommandFactory;
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
            return deleteUserCommandFactory.GetCommand(typeof(CompanyIdentity), userId);
        }
    }
}
