using JobBoardPlatform.BLL.Search.Templates;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Offers
{
    /// <summary>
    /// Create offer search data based on current url
    /// </summary>
    public class CompanyPanelOfferSearchParametersFactory
        : SearchParamsUrlFactoryBase<CompanyPanelOfferSearchParameters>
    {
        public CompanyPanelOfferSearchParametersFactory(HttpRequest httpRequest)
            : base(httpRequest)
        {
        }

        protected override void AssignFilterParams(CompanyPanelOfferSearchParameters searchParams)
        {
            searchParams.IsShowPublished = !IsBoolFilter(OfferSearchUrlParameters.HidePublished);
            searchParams.IsShowShelved = !IsBoolFilter(OfferSearchUrlParameters.HideShelved);
            throw new Exception("Not implemented yet");
            searchParams.CompanyProfileId = 0;//httpRequest.Query[OfferSearchUrlParameters.SortCategory];
        }
    }
}
