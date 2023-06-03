using JobBoardPlatform.BLL.Search.Templates;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Applications
{
    public class CompanyPanelApplicationSearchParametersFactory
        : SearchParamsUrlFactoryBase<CompanyPanelApplicationSearchParameters>
    {
        protected override void AssignFilterParams(HttpRequest httpRequest, CompanyPanelApplicationSearchParameters searchParams)
        {
            searchParams.IsShowUnseen = IsBoolFilter(httpRequest, OfferSearchUrlParameters.ShowUnseen);
            searchParams.IsShowMustHire = IsBoolFilter(httpRequest, OfferSearchUrlParameters.ShowMustHire);
            searchParams.IsShowAverage = IsBoolFilter(httpRequest, OfferSearchUrlParameters.ShowAverage);
            searchParams.IsShowRejected = IsBoolFilter(httpRequest, OfferSearchUrlParameters.ShowRejected);
            throw new Exception("Not implemented yet");
            searchParams.OfferId = 0;//httpRequest.Query[OfferSearchUrlParameters.SortCategory];
        }
    }
}
