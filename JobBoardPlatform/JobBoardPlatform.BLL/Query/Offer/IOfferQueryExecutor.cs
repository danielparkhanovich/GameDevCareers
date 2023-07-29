using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Query.Identity
{
    public interface IOfferQueryExecutor
    {
        Task<JobOffer> GetOfferById(int id);
    }
}
