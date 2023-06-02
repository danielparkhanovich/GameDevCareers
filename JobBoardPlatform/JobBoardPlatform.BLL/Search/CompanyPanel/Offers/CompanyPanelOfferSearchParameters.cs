using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.Enums;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Offers
{
    public class CompanyPanelOfferSearchParameters : IPageSearchParams
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
