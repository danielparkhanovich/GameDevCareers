using JobBoardPlatform.BLL.Commands;
using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Controllers.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Admin;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [HttpPost("LogInto")]
        public async Task<IActionResult> LogInto(int userId)
        {
            var logIntoCommand = GetLogIntoCommand(userId);
            await logIntoCommand.Execute();
            return RedirectToAction("Home");
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int userId)
        {
            var deleteCommand = GetDeleteCommand(userId);
            await deleteCommand.Execute();
            return Ok();
        }

        protected abstract ICommand GetLogIntoCommand(int userId);

        protected abstract ICommand GetDeleteCommand(int userId);

        protected override abstract Task<CardsContainerViewModel> GetContainer();
    }
}
