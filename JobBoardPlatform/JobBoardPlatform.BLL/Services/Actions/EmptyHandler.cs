using JobBoardPlatform.BLL.Services.PageViews;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Actions
{
    public class EmptyHandler : ActionHandlerBase
    {
        public override bool IsActionDoneRecently(HttpRequest request)
        {
            return false;
        }

        public override void RegisterAction(HttpRequest request, HttpResponse response)
        {

        }
    }
}
