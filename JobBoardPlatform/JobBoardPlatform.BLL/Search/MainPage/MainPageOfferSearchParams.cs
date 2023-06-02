using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.Enums;
using Microsoft.IdentityModel.Tokens;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    public class MainPageOfferSearchParams : IPageSearchParams
    {
        public const int AllTechnologiesIndex = 0;

        public OfferType Type { get; set; }
        public int MainTechnology { get; set; }
        public string? SearchString { get; set; }
        public bool IsSalaryOnly { get; set; }
        public bool IsRemoteOnly { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public bool IsQueryParams
        {
            get =>
                Page != 1 ||
                Type != OfferType.Employment ||
                IsRemoteOnly ||
                IsSalaryOnly ||
                MainTechnology != AllTechnologiesIndex ||
                !SearchString.IsNullOrEmpty();
        }

        public string? SortCategory { get; set; } = null;
        public SortType? Sort { get; set; } = null;
        public bool[]? FilterToggles { get; set; } = null;
    }
}
