using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminPanelCompaniesContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<CompanyIdentity>
    {
        private readonly IRepository<CompanyIdentity> repository;
        private readonly CompanyPanelOfferSearchParameters searchParams;
        private int totalRecordsCount;


        public AdminPanelCompaniesContainerViewModelFactory(IRepository<CompanyIdentity> repository, 
            CompanyPanelOfferSearchParameters searchParams)
        {
            this.repository = repository;
            this.searchParams = searchParams;
        }

        protected override ContainerHeaderViewModel? GetHeader()
        {
            var headerViewModelFactory = new CompanyOfferHeaderViewModelFactory();
            return null;
        }

        protected override async Task<List<IContainerCard>> GetCardsAsync()
        {
            var loaded = await Load();
            totalRecordsCount = loaded.Count;

            var cardFactory = new AdminCompanyCardViewModelFactory();

            return GetCards(cardFactory, loaded);
        }

        protected override IPageSearchParams GetSearchParams()
        {
            return searchParams;
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecordsCount;
        }

        private async Task<List<CompanyIdentity>> Load()
        {
            var query = await repository.GetAllSet();
            var records = await query
                .Include(application => application.Profile)
                .Take(20)
                .ToListAsync();

            return records;
        }
    }
}
