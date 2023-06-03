using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Search.Contracts
{
    public interface IPageSearchParamsUrlFactory<T> where T : class, IPageSearchParams, new()
    {
        public T GetSearchParams(HttpRequest httpRequest);
    }
}
