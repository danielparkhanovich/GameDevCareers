namespace JobBoardPlatform.BLL.Search
{
    public class OfferSearchUrlParameters
    {
        // Offers
        public const string Technology = "maintechnology";
        public const string SalaryOnly = "salary";
        public const string RemoteOnly = "remote";
        public const string Search = "search";
        public const string HidePublished = "hidepublished";
        public const string HideShelved = "hideshelved";

        // General
        public const string Page = "page";
        public const string Sort = "sort";
        public const string SortCategory = "sortcategory";

        // Applications
        public const string ShowUnseen = "showunseen";
        public const string ShowMustHire = "showmusthire";
        public const string ShowAverage = "showaverage";
        public const string ShowRejected = "showrejected";
    }
}
