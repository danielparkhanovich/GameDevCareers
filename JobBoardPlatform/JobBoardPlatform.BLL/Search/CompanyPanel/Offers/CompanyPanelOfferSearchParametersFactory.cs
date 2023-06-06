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
        protected override void AssignFilterParams(HttpRequest httpRequest, CompanyPanelOfferSearchParameters searchParams)
        {
            searchParams.IsShowPublished = !IsBoolFilter(httpRequest, OfferSearchUrlParams.HidePublished);
            searchParams.IsShowShelved = !IsBoolFilter(httpRequest, OfferSearchUrlParams.HideShelved);
            throw new Exception("Not implemented yet");
            searchParams.CompanyProfileId = 0;//httpRequest.Query[OfferSearchUrlParameters.SortCategory];
        }
    }
}
