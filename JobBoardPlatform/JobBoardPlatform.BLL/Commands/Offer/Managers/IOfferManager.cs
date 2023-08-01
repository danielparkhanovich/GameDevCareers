using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Offer
{
    public interface IOfferManager
    {
        public Task<List<int>> GetAllIdsAsync(int profileId);
        public Task<JobOffer> GetAsync(int offerId);
        public Task AddAsync(int profileId, IOfferData offerData);
        public Task UpdateAsync(IOfferData offerData);
        public Task DeleteAsync(int offerId);
        public Task ShelveAsync(int offerId, bool flag);
        public Task SuspendAsync(int offerId, bool flag);
        public Task PassPaymentAsync(int offerId);
    }
}
