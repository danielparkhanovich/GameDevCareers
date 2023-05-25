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
            string? sortCategory = GetSortCategoryType();
            SortType? sort = GetSortType();
            int page = GetPage();

            return new CompanyPanelOfferSearchParameters()
            {
                CompanyProfileId = companyProfileId,
                IsShowPublished = !isHidePublished,
                IsShowShelved = !isHideShelved,
                SortCategory = sortCategory,
                Sort = sort,
                Page = page,
                PageSize = 20
            };
        }

        private string GetSortCategoryType()
        {
            string? sortCategory = request.Query[OfferSearchUrlParameters.SortCategory];
            if (sortCategory == null)
            {
                return SortCategoryType.PublishDate.ToString();
            }

            return sortCategory;
        }

        private SortType? GetSortType()
        {
            string? sort = request.Query[OfferSearchUrlParameters.Sort];
            if (sort == null)
            {
                return SortType.Ascending;
            }

            SortType sortType = (SortType)Enum.Parse(typeof(SortType), sort, ignoreCase: true);
            return sortType;
        }

        private int GetPage()
        {
            string? pageString = request.Query[OfferSearchUrlParameters.Page];
            int page = int.TryParse(pageString, out int intOutParameter) ? intOutParameter : 1;
            return page;
        }
    }
}
