using Azure.Core;

namespace JobBoardPlatform.BLL.Search
{
    public class OfferSearchUrlParameters
    {
        public const string Technology = "maintechnology";
        public const string SalaryOnly = "salary";
        public const string RemoteOnly = "remote";
        public const string Search = "search";
        public const string Page = "page";
        public const string Sort = "sort";
        public const string SortCategory = "sortcategory";
        public const string HidePublished = "hidepublished";
        public const string HideShelved = "hideshelved";
    }
}
