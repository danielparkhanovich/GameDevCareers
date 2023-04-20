using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.PageViews
{
    public interface IViewsHandler
    {
        bool IsViewedRecently(int offerId, HttpRequest request, HttpResponse response);
    }
}
