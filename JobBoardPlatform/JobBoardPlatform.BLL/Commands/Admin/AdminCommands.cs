using JobBoardPlatform.BLL.Commands.Admin;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Search.MainPage;
using JobBoardPlatform.DAL.Models.Admin;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public class AdminCommands
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly UserManager<CompanyIdentity> companyManager;
        private readonly UserManager<EmployeeIdentity> employeeManager;
        private readonly UserManager<AdminIdentity> adminsManager;
        private readonly IOfferCacheManager cacheManager;
        private readonly MainPageOffersSearcher offersSearcher;
        private readonly IOfferManager offersManager;


        public AdminCommands(
            IRepository<JobOffer> offersRepository,
            UserManager<CompanyIdentity> companyManager,
            UserManager<EmployeeIdentity> employeeManager,
            UserManager<AdminIdentity> adminsManager,
            IOfferCacheManager cacheManager,
            MainPageOffersSearcher offersSearcher,
            IOfferManager offersManager)
        {
            this.offersRepository = offersRepository;
            this.companyManager = companyManager;
            this.employeeManager = employeeManager;
            this.adminsManager = adminsManager;
            this.cacheManager = cacheManager;
            this.offersSearcher = offersSearcher;
            this.offersManager = offersManager;
        }

        public async Task GenerateOffers(int companyId, int offersCountToGenerate)
        {
            var command = new GenerateOffersCommand(
                offersCountToGenerate, companyId, companyManager, offersRepository);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        public async Task DeleteAllUsers()
        {
            var deleteCompaniesCommand = new DeleteAllUsersCommand<CompanyIdentity>(companyManager);
            await deleteCompaniesCommand.Execute();

            var deleteEmployeesCommand = new DeleteAllUsersCommand<EmployeeIdentity>(employeeManager);
            await deleteEmployeesCommand.Execute();

            var deleteAdminsCommand = new DeleteAllUsersCommand<AdminIdentity>(adminsManager);
            await deleteAdminsCommand.Execute();

            await UpdateCacheAsync();
        }

        public async Task DeleteAllOffers()
        {
            var command = new DeleteAllOffersCommand(offersRepository, offersManager);
            await ExecuteCommandAndUpdateCacheAsync(command);
        }

        private async Task ExecuteCommandAndUpdateCacheAsync(ICommand command)
        {
            await command.Execute();
            await UpdateCacheAsync();
        }

        private async Task UpdateCacheAsync()
        {
            var mainPageParams = new MainPageOfferSearchParams();
            var searchResponse = await offersSearcher.Search(mainPageParams);
            await cacheManager.UpdateCache(searchResponse);
        }
    }
}
