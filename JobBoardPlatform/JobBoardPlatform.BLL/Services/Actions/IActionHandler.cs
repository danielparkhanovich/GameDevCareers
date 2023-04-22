using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.PageViews
{
    public interface IActionHandler
    {
        bool IsActionDoneRecently(HttpRequest request);
        void RegisterAction(HttpRequest request, HttpResponse response);
    }
}
