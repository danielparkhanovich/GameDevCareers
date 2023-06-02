using Azure.Core;
using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.BLL.Search.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.Templates
{
    public abstract class SearchParamsUrlFactoryBase<T> : IPageSearchParamsFactory<T>
        where T : class, IPageSearchParams, new()
    {
        private readonly HttpRequest httpRequest;


        public SearchParamsUrlFactoryBase(HttpRequest httpRequest)
        {
            this.httpRequest = httpRequest;
        }

        public T GetSearchParams()
        {
            var searchParams = new T();

            searchParams.SortCategory = GetSortCategoryType();
            searchParams.Sort = GetSortType();
            searchParams.Page = GetPage();

            AssignFilterParams(searchParams);
            return searchParams;
        }

        protected abstract void AssignFilterParams(T searchParams);

        protected bool IsBoolFilter(string filterName)
        {
            return httpRequest.Query.ContainsKey(filterName);
        }

        private string GetSortCategoryType()
        {
            string? sortCategory = httpRequest.Query[OfferSearchUrlParameters.SortCategory];
            if (sortCategory == null)
            {
                return SortCategoryType.PublishDate.ToString();
            }

            return sortCategory;
        }

        private SortType? GetSortType()
        {
            string? sort = httpRequest.Query[OfferSearchUrlParameters.Sort];
            if (sort == null)
            {
                return SortType.Ascending;
            }

            SortType sortType = (SortType)Enum.Parse(typeof(SortType), sort, ignoreCase: true);
            return sortType;
        }

        private int GetPage()
        {
            string? pageString = httpRequest.Query[OfferSearchUrlParameters.Page];
            int page = int.TryParse(pageString, out int intOutParameter) ? intOutParameter : 1;
            return page;
        }
    }
}
