using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Presenters
{
    public interface IViewRenderService
    {
        void SetController(Controller controller);
        Task<string> RenderPartialViewToString(string viewName, object model);
    }
}