using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Presenters
{
    public interface IViewRenderService
    {
        Task<string> RenderPartialViewToString(Controller Controller, string viewName, object model);
    }
}