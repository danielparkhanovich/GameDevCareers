using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.Temporary;
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
        private readonly IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService;
        private readonly IRepository<CompanyProfile> profileRepository;
        private readonly IOfferManager offersManager;
        private readonly IUserProfileImagesStorage imagesStorage;


        public AdminPanelCompaniesController(
            IRepository<CompanyIdentity> identityRepository,
            IRepository<CompanyProfile> profileRepository,
            IOfferManager offersManager,
            IUserProfileImagesStorage imagesStorage,
            IAuthorizationService<CompanyIdentity, CompanyProfile> authorizationService) 
            : base(identityRepository)
        {
            this.identityRepository = identityRepository;
            this.profileRepository = profileRepository;
            this.offersManager = offersManager;
            this.imagesStorage = imagesStorage;
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
            return new DeleteCompanyCommand(
                identityRepository,
                profileRepository,
                offersManager,
                imagesStorage,
                userId);
        }
    }
}
