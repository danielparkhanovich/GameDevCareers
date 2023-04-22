using JobBoardPlatform.BLL.Services.PageViews;

namespace JobBoardPlatform.BLL.Services.Actions.Offers.Factory
{
    public class OfferActionEmptyHandlerFactory : IOfferActionHandlerFactory
    {
        public IActionHandler GetViewActionHandler(int offerId)
        {
            return new EmptyHandler();
        }

        public IActionHandler GetApplyActionHandler(int offerId)
        {
            return new EmptyHandler();
        }
    }
}
