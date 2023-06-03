using Azure.Core;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.Templates
{
    public abstract class SearchParamsUrlFactoryBase<T> : IPageSearchParamsUrlFactory<T>
        where T : class, IPageSearchParams, new()
    {
        public T GetSearchParams(HttpRequest httpRequest)
        {
            var searchParams = new T();

            searchParams.SortCategory = GetSortCategoryType(httpRequest);
            searchParams.Sort = GetSortType(httpRequest);
            searchParams.Page = GetPage(httpRequest);

            AssignFilterParams(httpRequest, searchParams);
            return searchParams;
        }

        protected abstract void AssignFilterParams(HttpRequest httpRequest, T searchParams);

        protected bool IsBoolFilter(HttpRequest httpRequest, string filterName)
        {
            return httpRequest.Query.ContainsKey(filterName);
        }

        private string GetSortCategoryType(HttpRequest httpRequest)
        {
            string? sortCategory = httpRequest.Query[OfferSearchUrlParameters.SortCategory];
            if (sortCategory == null)
            {
                return SortCategoryType.PublishDate.ToString();
            }

            return sortCategory;
        }

        private SortType? GetSortType(HttpRequest httpRequest)
        {
            string? sort = httpRequest.Query[OfferSearchUrlParameters.Sort];
            if (sort == null)
            {
                return SortType.Ascending;
            }

            SortType sortType = (SortType)Enum.Parse(typeof(SortType), sort, ignoreCase: true);
            return sortType;
        }

        private int GetPage(HttpRequest httpRequest)
        {
            string? pageString = httpRequest.Query[OfferSearchUrlParameters.Page];
            int page = int.TryParse(pageString, out int intOutParameter) ? intOutParameter : 1;
            return page;
        }
    }
}
