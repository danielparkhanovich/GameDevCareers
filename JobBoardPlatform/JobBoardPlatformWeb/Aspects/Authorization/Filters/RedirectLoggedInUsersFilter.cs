using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.PL.Controllers.Register;
using JobBoardPlatform.PL.Controllers.Presenters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoardPlatform.PL.Filters
{
    public class RedirectLoggedInUsersFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (UserSessionUtils.IsLoggedIn(context.HttpContext.User) && 
                UserSessionUtils.IsHasAnyRole(context.HttpContext.User))
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
