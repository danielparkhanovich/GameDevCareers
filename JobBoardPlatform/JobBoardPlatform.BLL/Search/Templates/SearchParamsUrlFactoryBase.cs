using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.Templates
{
    public abstract class SearchParamsUrlFactoryBase<T> : IPageSearchParamsUrlFactory<T>
        where T : class, IPageSearchParams, new()
    {
        public T GetSearchParams(HttpRequest request)
        {
            var searchParams = new T();

            searchParams.SortCategory = GetSortCategoryType(request);
            searchParams.Sort = GetSortType(request);
            searchParams.Page = GetPage(request);

            AssignFilterParams(request, searchParams);
            return searchParams;
        }

        protected abstract void AssignFilterParams(HttpRequest request, T searchParams);

        protected bool IsBoolFilter(HttpRequest request, string filterName)
        {
            return request.Query.ContainsKey(filterName);
        }

        protected int GetIntFilter(HttpRequest request, string filterName)
        {
            string? filterString = request.Query[filterName];
            if (string.IsNullOrEmpty(filterString))
            {
                // filterString = request.RouteValues[filterName];
            }
            return int.Parse(filterString!);
        }

        private string GetSortCategoryType(HttpRequest request)
        {
            string? sortCategory = request.Query[OfferSearchUrlParams.SortCategory];
            if (sortCategory == null)
            {
                return SortCategoryType.PublishDate.ToString();
            }

            return sortCategory;
        }

        private SortType? GetSortType(HttpRequest request)
        {
            string? sort = request.Query[OfferSearchUrlParams.Sort];
            if (sort == null)
            {
                return SortType.Ascending;
            }

            SortType sortType = (SortType)Enum.Parse(typeof(SortType), sort, ignoreCase: true);
            return sortType;
        }

        private int GetPage(HttpRequest request)
        {
            string? pageString = request.Query[OfferSearchUrlParams.Page];
            int page = int.TryParse(pageString, out int intOutParameter) ? intOutParameter : 1;
            return page;
        }
    }
}
