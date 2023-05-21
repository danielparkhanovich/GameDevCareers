using JobBoardPlatform.BLL.Search.Enums;
using Microsoft.IdentityModel.Tokens;

namespace JobBoardPlatform.BLL.Search.MainPage
{
    public class MainPageOfferSearchParameters : ISearchParameters
    {
        public const int AllTechnologiesIndex = 0;

        public OfferType Type { get; set; }
        public int MainTechnology { get; set; }
        public string? SearchString { get; set; }
        public bool IsSalaryOnly { get; set; }
        public bool IsRemoteOnly { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

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

        public string? SortCategory => null;

        public SortType? Sort => null;

        public bool[]? FilterToggles => null;
    }
}
