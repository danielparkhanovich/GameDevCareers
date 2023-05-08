using Azure;

namespace JobBoardPlatform.BLL.Search.Offers
{
    public class OfferSearchData
    {
        public OfferType Type { get; set; }
        public int MainTechnology { get; set; }
        public string? SearchString { get; set; }
        public bool IsSalaryOnly { get; set; }
        public bool IsRemoteOnly { get; set; }
        public int Page { get; set; }

        public bool IsQueryParams { get => 
                (Page != 1 ||
                Type != OfferType.Employment ||
                IsRemoteOnly ||
                IsSalaryOnly ||
                MainTechnology != 0);
        }
    }
}
