using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace JobBoardPlatform.BLL.Services.PageViews
{
    public abstract class ActionHandlerBase : IActionHandler
    {
        public abstract bool IsActionDoneRecently(HttpRequest request);

        public abstract void RegisterAction(HttpRequest request, HttpResponse response);

        protected void RegisterAction(HttpRequest request, HttpResponse response,
            string cookieName, int cooldownInMinutes)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(cooldownInMinutes)
            };

            // refresh cookie
            var viewedCookie = request.Cookies[cookieName];
            if (viewedCookie != null)
            {
                response.Cookies.Delete(cookieName);
            }
            response.Cookies.Append(cookieName, DateTime.Now.ToString(), options);

            // refresh ip address in cache
            SaveUserIpAddress(request, cookieName, cooldownInMinutes);
        }

        protected bool IsActionDoneRecently(HttpRequest request, string cookieName, int cooldownInMinutes)
        {
            // check view cookie at the first (fast action reject)
            var viewedCookie = request.Cookies[cookieName];
            if (viewedCookie != null)
            {
                DateTime lastViewed = DateTime.Parse(viewedCookie);
                if (DateTime.Now.Subtract(lastViewed).TotalMinutes < cooldownInMinutes)
                {
                    return true;
                }
            }

            // one more security layer memory cache
            string ipAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            if (cache.TryGetValue(ipAddress, out bool _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SaveUserIpAddress(HttpRequest request, string cookieName, int cacheExpireTimeInMinutes)
        {
            string ipAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();

            string relatedActionKey = $"{ipAddress}_{cookieName}";

            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            cache.Set(relatedActionKey, true, DateTimeOffset.Now.AddMinutes(cacheExpireTimeInMinutes));
        }
    }
}
