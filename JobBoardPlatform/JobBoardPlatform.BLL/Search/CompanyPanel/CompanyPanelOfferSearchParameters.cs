using JobBoardPlatform.BLL.Search.Enums;

namespace JobBoardPlatform.BLL.Search.CompanyPanel
{
    public class CompanyPanelOfferSearchParameters : ISearchParameters
    {
        public int? CompanyProfileId { get; set; }
        public bool IsShowPublished { get; set; } = true;
        public bool IsShowShelved { get; set; } = true;
        public string? SortCategory { get; set; } = SortCategoryType.PublishDate.ToString();
        public SortType? Sort { get; set; } = SortType.Descending;
        public bool[]? FilterToggles => new bool[2] { IsShowPublished, IsShowShelved };
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
