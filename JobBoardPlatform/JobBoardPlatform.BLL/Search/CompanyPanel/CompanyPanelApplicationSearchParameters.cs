using JobBoardPlatform.BLL.Search.Enums;

namespace JobBoardPlatform.BLL.Search.CompanyPanel
{
    public class CompanyPanelApplicationSearchParameters : ISearchParameters
    {
        public int OfferId { get; set; }
        public bool IsShowUnseen { get; set; } = true;
        public bool IsShowMustHire { get; set; } = true;
        public bool IsShowAverage { get; set; } = true;
        public bool IsShowRejected { get; set; } = true;

        public string? SortCategory { get; set; } = SortCategoryType.PublishDate.ToString();
        public SortType? Sort { get; set; } = SortType.Descending;
        public bool[]? FilterToggles => new bool[4] { IsShowUnseen, IsShowMustHire, IsShowAverage, IsShowRejected };
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
