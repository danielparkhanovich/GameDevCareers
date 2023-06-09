using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
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
        private readonly IRepository<CompanyProfile> profileRepository;


        public AdminPanelCompaniesController(
            IRepository<CompanyIdentity> identityRepository, IRepository<CompanyProfile> profileRepository) 
            : base(identityRepository)
        {
            this.identityRepository = identityRepository;
            this.profileRepository = profileRepository;
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
                HttpContext, identityRepository, profileRepository, userId);
        }

        protected override ICommand GetDeleteCommand(int userId)
        {
            return new DeleteCompanyCommand(identityRepository, userId);
        }
    }
}
