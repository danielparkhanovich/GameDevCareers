using JobBoardPlatform.BLL.Search.Templates;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Applications
{
    public class CompanyPanelApplicationSearchParamsFactory
        : SearchParamsUrlFactoryBase<CompanyPanelApplicationSearchParams>
    {
        protected override void AssignFilterParams(
            HttpRequest request, CompanyPanelApplicationSearchParams searchParams)
        {
            searchParams.IsShowUnseen = IsBoolFilter(request, OfferSearchUrlParams.ShowUnseen);
            searchParams.IsShowMustHire = IsBoolFilter(request, OfferSearchUrlParams.ShowMustHire);
            searchParams.IsShowAverage = IsBoolFilter(request, OfferSearchUrlParams.ShowAverage);
            searchParams.IsShowRejected = IsBoolFilter(request, OfferSearchUrlParams.ShowRejected);
            searchParams.OfferId = GetIntFilter(request, OfferSearchUrlParams.OfferId);
        }
    }
}
