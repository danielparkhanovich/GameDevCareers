using JobBoardPlatform.BLL.Search.CompanyPanel.Offers;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminPanelEmployeesContainerViewModelFactory : CardsContainerViewModelFactoryTemplate<EmployeeIdentity>
    {
        private readonly IRepository<EmployeeIdentity> repository;
        private int totalRecordsCount;


        public AdminPanelEmployeesContainerViewModelFactory(IRepository<EmployeeIdentity> repository)
        {
            this.repository = repository;
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

            var cardFactory = new AdminEmployeeCardViewModelFactory();

            return GetCards(cardFactory, loaded);
        }

        protected override IPageSearchParams GetSearchParams()
        {
            return new CompanyPanelOfferSearchParameters();
        }

        protected override int GetTotalRecordsCount()
        {
            return totalRecordsCount;
        }

        private async Task<List<EmployeeIdentity>> Load()
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
