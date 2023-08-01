using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
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
    [Route("admin-panel-offers")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminPanelOffersController : OfferCardsControllerBase
    {
        private readonly IRepository<CompanyProfile> companyRepository;
        private readonly CompanyOffersSearcher offersSearcher;
        private readonly AdminCommandsExecutor adminCommandsExecutor;
        private readonly IOfferQueryExecutor queryExecutor;


        public AdminPanelOffersController(
            IRepository<CompanyProfile> companyRepository,
            IOfferManager commandsExecutor,
            AdminCommandsExecutor adminCommandsExecutor,
            CompanyOffersSearcher offersSearcher,
            IOfferQueryExecutor queryExecutor) 
            : base(commandsExecutor)
        {
            this.companyRepository = companyRepository;
            this.offersSearcher = offersSearcher;
            this.adminCommandsExecutor = adminCommandsExecutor;
            this.queryExecutor = queryExecutor;
        }

        public async Task<IActionResult> Panel()
        {
            var viewModel = new AdminPanelOffersViewModel();
            viewModel.CardsContainer = await GetContainer();
            viewModel.AllRecords = await companyRepository.GetAll();

            return View(viewModel);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateOffers(AdminPanelOffersViewModel form)
        {
            int count = form.CountToGenerate;

            bool isOverLimit = await IsGenerateCountOverLimit(count);
            if (isOverLimit)
            {
                return RedirectToAction("Panel");
            }

            await adminCommandsExecutor.GenerateOffers(form.CompanyIdToGenerate, count);
            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllOffers()
        {
            await adminCommandsExecutor.DeleteAllOffers();
            return RedirectToAction("Panel");
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var searchParamsFactory = new CompanyPanelOfferSearchParametersFactory();
            var searchParams = searchParamsFactory.GetSearchParams(Request);

            var containerFactory = new AdminPanelOffersContainerViewModelFactory(
                offersSearcher, searchParams);
            return containerFactory.CreateAsync();
        }

        protected override Task<JobOffer> GetLoadedOffer(int offerId)
        {
            return queryExecutor.GetOfferById(offerId);
        }

        protected override IContainerCard GetContainerCard(JobOffer offer)
        {
            var cardFactory = new AdminOfferViewModelFactory();
            return cardFactory.CreateCard(offer);
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
