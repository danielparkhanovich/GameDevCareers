using JobBoardPlatform.BLL.Common.Contracts;
using JobBoardPlatform.BLL.Search.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.CompanyPanel
{
    /// <summary>
    /// Create offer search data based on current url
    /// </summary>
    public class CompanyPanelOfferSearchParametersFactory : IFactory<CompanyPanelOfferSearchParameters>
    {
        private readonly HttpRequest request;
        private readonly int? companyProfileId;


        public CompanyPanelOfferSearchParametersFactory(HttpRequest request, int? companyProfileId = null)
        {
            this.request = request;
            this.companyProfileId = companyProfileId;
        }

        public CompanyPanelOfferSearchParameters Create()
        {
            // Get all query parameters
            bool isHidePublished = request.Query.ContainsKey(OfferSearchUrlParameters.HidePublished);
            bool isHideShelved = request.Query.ContainsKey(OfferSearchUrlParameters.HideShelved);
            string? sortCategory = request.Query[OfferSearchUrlParameters.SortCategory];
            string? sort = request.Query[OfferSearchUrlParameters.Sort];
            string? pageString = request.Query[OfferSearchUrlParameters.Page];

            int page = int.TryParse(pageString, out int intOutParameter) ? intOutParameter : 1;

            return new CompanyPanelOfferSearchParameters()
            {
                CompanyProfileId = companyProfileId,
                IsShowPublished = !isHidePublished,
                IsShowShelved = !isHideShelved,
                SortCategory = sortCategory,
                Sort = GetSortType(sort),
                Page = page,
                PageSize = 20
            };
        }

        private SortType? GetSortType(string? sort)
        {
            if (sort == null)
            {
                return null;
            }

            SortType sortType = (SortType)Enum.Parse(typeof(SortType), sort, ignoreCase: true);
            return sortType;
        }
    }
}
