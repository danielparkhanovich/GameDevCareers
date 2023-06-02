using JobBoardPlatform.BLL.Search.Enums;

namespace JobBoardPlatform.BLL.Search.Contracts
{
    public interface IPageSearchParams
    {
        string? SortCategory { get; set; }
        SortType? Sort { get; set; }
        int Page { get; set; }
        bool[]? FilterToggles { get; }
        int PageSize { get; }
    }
}
