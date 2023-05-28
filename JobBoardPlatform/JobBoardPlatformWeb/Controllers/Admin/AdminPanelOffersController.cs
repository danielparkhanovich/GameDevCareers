using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Search.CompanyPanel;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Controllers.Offer;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminPanelOffersController : OfferCardsControllerBase
    {
        private readonly IRepository<JobOffer> offerRepository;
        private readonly IRepository<CompanyProfile> companyRepository;
        private readonly IRepository<CompanyIdentity> companyIdentityRepository;
        private readonly IRepository<TechKeyword> keywordsRepository;


        public AdminPanelOffersController(IRepository<JobOffer> offerRepository,
            IRepository<CompanyProfile> companyRepository,
            IRepository<CompanyIdentity> companyIdentityRepository,
            IRepository<TechKeyword> keywordsRepository,
            OfferCommandsExecutor commandsExecutor) 
            : base(commandsExecutor)
        {
            this.offerRepository = offerRepository;
            this.companyRepository = companyRepository;
            this.companyIdentityRepository = companyIdentityRepository;
            this.keywordsRepository = keywordsRepository;
        }

        public async Task<IActionResult> Panel()
        {
            var viewModel = new AdminPanelOffersViewModel();
            viewModel.CardsContainer = await GetContainer();
            viewModel.AllRecords = await companyRepository.GetAll();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOffers(int companyId, int offersCountToGenerate)
        {
            bool isOverLimit = await IsGenerateCountOverLimit(offersCountToGenerate);
            if (isOverLimit)
            {
                return RedirectToAction("Panel");
            }

            var generateOffersCommand = new GenerateOffersCommand(offersCountToGenerate,
                companyId, 
                companyIdentityRepository, 
                keywordsRepository, 
                offerRepository);
            await generateOffersCommand.Execute();

            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllOffers()
        {
            var deleteOffersCommand = new DeleteAllOffersCommand(offerRepository);
            await deleteOffersCommand.Execute();

            return RedirectToAction("OffersPanel");
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var searchParameters = GetSearchParametersFromUrl();
            var containerFactory = new AdminPanelOffersContainerViewModelFactory(offerRepository, searchParameters!);
            return containerFactory.Create();
        }

        protected override Task<JobOffer> GetLoadedOffer(int offerId)
        {
            var loader = new LoadCompanyOffer(offerRepository, offerId);
            return loader.Load();
        }

        protected override IContainerCard GetContainerCard(JobOffer offer)
        {
            var cardFactory = new AdminOfferViewModelFactory();
            return cardFactory.CreateCard(offer);
        }

        private CompanyPanelOfferSearchParameters GetSearchParametersFromUrl()
        {
            int? profileId = null;
            var offerSearchParametersFactory = new CompanyPanelOfferSearchParametersFactory(Request, profileId);
            var searchParams = offerSearchParametersFactory.Create();

            return searchParams;
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
    }
}
