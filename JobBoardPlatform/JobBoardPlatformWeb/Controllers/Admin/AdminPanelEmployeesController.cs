using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Factories.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Route("admin-panel-employees")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public class AdminPanelEmployeesController : AdminPanelUsersControllerBase<EmployeeIdentity, AdminPanelEmployeesViewModel>
    {
        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IRepository<EmployeeProfile> profileRepository;


        public AdminPanelEmployeesController(
            IRepository<EmployeeIdentity> identityRepository, IRepository<EmployeeProfile> profileRepository)
            : base(identityRepository)
        {
            this.identityRepository = identityRepository;
            this.profileRepository = profileRepository;
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var containerFactory = new AdminPanelEmployeesContainerViewModelFactory(identityRepository);
            return containerFactory.CreateAsync();
        }

        protected override ICommand GetLogIntoCommand(int userId)
        {
            return new LogIntoAccountCommand<EmployeeIdentity, EmployeeProfile>(
                HttpContext, identityRepository, profileRepository, userId);
        }

        protected override ICommand GetDeleteCommand(int userId)
        {
            return new DeleteEmployeeCommand(identityRepository, userId);
        }
    }
}
