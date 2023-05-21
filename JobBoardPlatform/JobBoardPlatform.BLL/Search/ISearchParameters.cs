using JobBoardPlatform.BLL.Search.Enums;

namespace JobBoardPlatform.BLL.Search
{
    public interface ISearchParameters
    {
        string? SortCategory { get; }
        SortType? Sort { get; }
        bool[]? FilterToggles { get; }
        int Page { get; }
        int PageSize { get; }
    }
}
