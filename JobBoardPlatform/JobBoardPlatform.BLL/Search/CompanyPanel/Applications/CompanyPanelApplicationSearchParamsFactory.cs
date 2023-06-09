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
            searchParams.IsShowUnseen = !IsBoolFilter(request, OfferSearchUrlParams.HideUnseen);
            searchParams.IsShowMustHire = !IsBoolFilter(request, OfferSearchUrlParams.HideMustHire);
            searchParams.IsShowAverage = !IsBoolFilter(request, OfferSearchUrlParams.HideAverage);
            searchParams.IsShowRejected = !IsBoolFilter(request, OfferSearchUrlParams.HideRejected);
        }
    }
}
