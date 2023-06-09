using Azure.Core;
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
        protected override void AssignFilterParams(
            HttpRequest request, CompanyPanelOfferSearchParameters searchParams)
        {
            searchParams.IsShowPublished = !IsBoolFilter(request, OfferSearchUrlParams.HidePublished);
            searchParams.IsShowShelved = !IsBoolFilter(request, OfferSearchUrlParams.HideShelved);
        }
    }
}
