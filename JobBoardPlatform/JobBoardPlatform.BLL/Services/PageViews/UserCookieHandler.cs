using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace JobBoardPlatform.BLL.Services.PageViews
{
    public class UserViewsHandler : IViewsHandler
    {
        public bool IsViewedRecently(int offerId, HttpRequest request, HttpResponse response)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60)
            };

            string cookieName = $"OfferViewed_{offerId}";

            var viewedCookie = request.Cookies[$"OfferViewed_{offerId}"];
            if (viewedCookie != null)
            {
                DateTime lastViewed = DateTime.Parse(viewedCookie);
                if (DateTime.Now.Subtract(lastViewed).TotalMinutes < 60)
                {
                    return true;
                }
            }

            if (viewedCookie == null)
            {
                response.Cookies.Append(cookieName, DateTime.Now.ToString(), options);
            }

            // Prevent cookies deletion
            string ipAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            if (cache.TryGetValue(ipAddress, out bool _))
            {
                return true;
            }
            else
            {
                SaveUserIpAddress(request);
                return false;
            }
        }

        private void SaveUserIpAddress(HttpRequest request)
        {
            string ipAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            cache.Set(ipAddress, true, DateTimeOffset.Now.AddMinutes(60));
        }
    }
}
