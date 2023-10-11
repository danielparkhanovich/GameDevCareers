using JobBoardPlatform.BLL.Services.PageViews;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Services.Actions.Offers.Factory
{
    public class OfferActionHandlerFactory : IOfferActionHandlerFactory
    {
        public IActionHandler GetViewActionHandler(int offerId)
        {
            return new OfferViewActionHandler(offerId);
        }

        public IActionHandler GetApplyActionHandler(int offerId)
        {
            return new OfferApplyActionHandler(offerId);
        }
    }
}
