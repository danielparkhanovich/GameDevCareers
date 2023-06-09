using JobBoardPlatform.PL.ViewModels.Models.Templates;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Templates
{
    public abstract class CardsControllerBase : Controller
    {
        public const string RefreshCardsContainerAction = "RefreshCardsContainer";


        [HttpPost("[action]")]
        public virtual async Task<IActionResult> RefreshCardsContainer()
        {
            var container = await GetContainer();
            var test = Request;
            return PartialView(CardsContainerViewModel.PartialView, container);
        }

        protected abstract Task<CardsContainerViewModel> GetContainer();
    }
}
