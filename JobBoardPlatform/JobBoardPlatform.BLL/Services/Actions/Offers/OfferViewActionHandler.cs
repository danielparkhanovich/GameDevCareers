using JobBoardPlatform.BLL.Services.PageViews;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Actions.Offers
{
    public class OfferViewActionHandler : ActionHandlerBase
    {
        private const int ViewCooldownInMinutes = 60;

        private readonly string cookieName;


        public OfferViewActionHandler(int offerId) 
        {
            this.cookieName = $"OfferViewed_{offerId}";
        }

        public override bool IsActionDoneRecently(HttpRequest request)
        {
            return IsActionDoneRecently(request, cookieName, ViewCooldownInMinutes);
        }

        public override void RegisterAction(HttpRequest request, HttpResponse response)
        {
            RegisterAction(request, response, cookieName, ViewCooldownInMinutes);
        }
    }
}
