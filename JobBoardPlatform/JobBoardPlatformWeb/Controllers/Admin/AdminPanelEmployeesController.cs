using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
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
        private readonly IDeleteUserCommandFactory deleteUserCommandFactory;
        private readonly IRepository<EmployeeIdentity> identityRepository;
        private readonly IAuthorizationService<EmployeeIdentity, EmployeeProfile> authorizationService;


        public AdminPanelEmployeesController(
            IDeleteUserCommandFactory deleteUserCommandFactory,
            IRepository<EmployeeIdentity> identityRepository,
            IAuthorizationService<EmployeeIdentity, EmployeeProfile> authorizationService)
            : base(identityRepository)
        {
            this.deleteUserCommandFactory = deleteUserCommandFactory;
            this.identityRepository = identityRepository;
            this.authorizationService = authorizationService;
        }

        protected override Task<CardsContainerViewModel> GetContainer()
        {
            var containerFactory = new AdminPanelEmployeesContainerViewModelFactory(identityRepository);
            return containerFactory.CreateAsync();
        }

        protected override ICommand GetLogIntoCommand(int userId)
        {
            return new LogIntoAccountCommand<EmployeeIdentity, EmployeeProfile>(
                HttpContext, authorizationService, userId);
        }

        protected override ICommand GetDeleteCommand(int userId)
        {
            return deleteUserCommandFactory.GetCommand(typeof(EmployeeIdentity), userId);
        }
    }
}
