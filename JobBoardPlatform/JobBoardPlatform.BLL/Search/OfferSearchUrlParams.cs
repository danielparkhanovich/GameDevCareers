namespace JobBoardPlatform.BLL.Search
{
    public class OfferSearchUrlParams
    {
        // Offers
        public const string Technology = "maintechnology";
        public const string SalaryOnly = "salary";
        public const string RemoteOnly = "remote";
        public const string Search = "search";
        public const string HidePublished = "hidepublished";
        public const string HideShelved = "hideshelved";
        public const string OfferId = "offerId";

        // General
        public const string Page = "page";
        public const string Sort = "sort";
        public const string SortCategory = "sortcategory";

        // Applications
        public const string HideUnseen = "hideunseen";
        public const string HideMustHire = "hidemusthire";
        public const string HideAverage = "hideaverage";
        public const string HideRejected = "hiderejected";
    }
}
