using JobBoardPlatform.BLL.Services.PageViews;

namespace JobBoardPlatform.BLL.Services.Actions.Offers.Factory
{
    public interface IOfferActionHandlerFactory
    {
        public IActionHandler GetViewActionHandler(int offerId);
        public IActionHandler GetApplyActionHandler(int offerId);
    }
}
