using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Profile
{
    [Authorize(Policy = AuthorizationPolicies.AdminOnlyPolicy)]
    public abstract class AdminPanelUsersControllerBase<TEntity, TViewModel> : CardsControllerBase
        where TEntity: class, IEntity
        where TViewModel: IAdminPanelViewModel<TEntity>, new()
    {
        public const string LogIntoAction = "LogInto";
        public const string DeleteAction = "Delete";

        private readonly IRepository<TEntity> identityRepository;


        public AdminPanelUsersControllerBase(IRepository<TEntity> identityRepository)
        {
            this.identityRepository = identityRepository;
        }

        public async Task<IActionResult> Panel()
        {
            var viewModel = new TViewModel();
            viewModel.CardsContainer = await GetContainer();
            viewModel.AllRecords = await identityRepository.GetAll();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogInto(int userId)
        {
            var logIntoCommand = GetLogIntoCommand(userId);
            await logIntoCommand.Execute();
            return RedirectToAction("Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            var deleteCommand = GetDeleteCommand(userId);
            await deleteCommand.Execute();
            return RedirectToAction("Panel");
        }

        protected abstract ICommand GetLogIntoCommand(int userId);

        protected abstract ICommand GetDeleteCommand(int userId);

        protected override abstract Task<CardsContainerViewModel> GetContainer();
    }
}
