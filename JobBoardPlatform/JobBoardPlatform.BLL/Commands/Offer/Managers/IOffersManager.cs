using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public interface IOffersManager
    {
        public Task<List<int>> GetAsync(int profileId);
        public Task AddAsync(int profileId, INewOfferData offerData);
        public Task DeleteAsync(int offerId);
        public Task ShelveAsync(int offerId, bool flag);
        public Task SuspendAsync(int offerId, bool flag);
        public Task PassPaymentAsync(int offerId);
    }
}
