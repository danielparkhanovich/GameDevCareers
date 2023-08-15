using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Query.Identity
{
    public interface IOfferPlanQueryExecutor
    {
        Task<List<JobOfferPlan>> GetAllAsync();
    }
}
