using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Utils.Renderers
{
    public interface IViewRenderService
    {
        Task<string> RenderPartialViewToString(Controller Controller, string viewName, object model);
    }
}