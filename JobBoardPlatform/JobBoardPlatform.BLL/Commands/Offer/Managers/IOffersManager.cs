using JobBoardPlatform.BLL.Models.Contracts;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public interface IOffersManager
    {
        public Task AddAsync(int profileId, INewOfferData offerData);
        public Task DeleteAsync(int offerId);
        public Task ShelveAsync(int offerId, bool flag);
        public Task SuspendAsync(int offerId, bool flag);
        public Task PassPaymentAsync(int offerId);
    }
}
