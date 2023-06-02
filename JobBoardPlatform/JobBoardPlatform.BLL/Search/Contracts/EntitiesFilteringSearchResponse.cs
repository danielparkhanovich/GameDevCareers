using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Search.Contracts
{
    public class EntitiesFilteringSearchResponse<T> where T : class, IEntity
    {
        public int TotalRecordsAfterFilters { get; set; }
        public List<T> Entities { get; set; } = new List<T>();
    }
}
