using JobBoardPlatform.BLL.Services.PageViews;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Actions.Offers
{
    public class OfferApplyActionHandler : ActionHandlerBase
    {
        private const int ApplyCooldownInMinutes = 30 * 24 * 60; // 30 days

        private readonly string cookieName;


        public OfferApplyActionHandler(int offerId) 
        {
            this.cookieName = $"OfferApplied_{offerId}";
        }

        public override bool IsActionDoneRecently(HttpRequest request)
        {
            return IsActionDoneRecently(request, cookieName, ApplyCooldownInMinutes);
        }

        public override void RegisterAction(HttpRequest request, HttpResponse response)
        {
            RegisterAction(request, response, cookieName, ApplyCooldownInMinutes);
        }
    }
}
