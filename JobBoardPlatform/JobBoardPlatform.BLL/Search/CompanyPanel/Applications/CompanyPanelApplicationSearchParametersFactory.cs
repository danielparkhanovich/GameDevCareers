using JobBoardPlatform.BLL.Search.Templates;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.CompanyPanel.Applications
{
    public class CompanyPanelApplicationSearchParametersFactory
        : SearchParamsUrlFactoryBase<CompanyPanelApplicationSearchParameters>
    {
        public CompanyPanelApplicationSearchParametersFactory(HttpRequest httpRequest)
            : base(httpRequest)
        {
        }

        protected override void AssignFilterParams(CompanyPanelApplicationSearchParameters searchParams)
        {
            searchParams.IsShowUnseen = IsBoolFilter(OfferSearchUrlParameters.ShowUnseen);
            searchParams.IsShowMustHire = IsBoolFilter(OfferSearchUrlParameters.ShowMustHire);
            searchParams.IsShowAverage = IsBoolFilter(OfferSearchUrlParameters.ShowAverage);
            searchParams.IsShowRejected = IsBoolFilter(OfferSearchUrlParameters.ShowRejected);
            throw new Exception("Not implemented yet");
            searchParams.OfferId = 0;//httpRequest.Query[OfferSearchUrlParameters.SortCategory];
        }
    }
}
